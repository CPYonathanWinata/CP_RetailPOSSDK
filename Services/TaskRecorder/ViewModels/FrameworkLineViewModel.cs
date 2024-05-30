/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Dynamics.Retail.Pos.DataManager;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MVVM;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Services;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels
{
    /// <summary>
    /// Model for framework line
    /// </summary>
    public class FrameworkLineViewModel : NotifyPropertyChangedBase
    {
        #region property backend fields

        private ObservableCollection<FrameworkLineViewModel> children;
        private RecorderLineOrigin? origin;
        private bool allowArtifacts;
        private bool allowChildNodes;
        private static FrameworkLineViewModelComparer comparer = new FrameworkLineViewModelComparer();

        #endregion

        #region private fields

        private string framework;
        private string industry;
        private string frameworkIndustryName;
        private static Dictionary<Guid, IGrouping<Guid, SysTaskRecorderLineServiceContract>> groupedLines;
        private static Dictionary<int, bool> allowLevelArtifacts;
        private static int maxAllowedLevel = 0;
        private static SysTaskRecorderMapServiceContract map;
        private FrameworkLineViewModel parent;
        private TaskRecorderService serviceManager = TaskRecorderService.Instance;

        #endregion

        #region Factory method

        /// <summary>
        /// Creates framework line hierarchy.
        /// </summary>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        /// <param name="frameworkIndustryMap">The framework industry map.</param>
        /// <returns>Framework line hierarchy.</returns>        
        public static ObservableCollection<FrameworkLineViewModel> Create(string frameworkId, string industryCode, SysTaskRecorderMapServiceContract frameworkIndustryMap)
        {
            groupedLines = null;
            allowLevelArtifacts = null;
            map = frameworkIndustryMap;

            if (map != null)
            {
                if (map.FrameworkLines != null)
                {
                    groupedLines = map.FrameworkLines.GroupBy(line => line.ParentId).ToDictionary(gr => gr.Key);
                }

                InitializeAllowLevelArtifacts(map);
            }
            return new ObservableCollection<FrameworkLineViewModel>() { new FrameworkLineViewModel(frameworkId, industryCode) };
        }

        #endregion

        #region constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="FrameworkLineViewModel" /> class from being created.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="parentLine">The parent line.</param>
        private FrameworkLineViewModel(SysTaskRecorderLineServiceContract line, FrameworkLineViewModel parentLine)
        {
            this.parent = parentLine;
            this.Model = line;
            InitializeFlag();

            if (this.Model != null)
            {
                this.AddChildLines(this.Model.SyncId);
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FrameworkLineViewModel"/> class from being created.
        /// </summary>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        private FrameworkLineViewModel(string frameworkId, string industryCode)
        {
            this.framework = frameworkId;
            this.industry = industryCode;
            this.InitializeFlag();

            if ((!string.IsNullOrWhiteSpace(this.framework)) && (!string.IsNullOrWhiteSpace(this.industry)))
            {
                this.frameworkIndustryName = string.Format("{0}_{1}", this.framework, this.industry);
            }

            this.AddChildLines(Guid.Empty);
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets the comparer.
        /// </summary>
        /// <value>
        /// The comparer.
        /// </value>
        public static FrameworkLineViewModelComparer Comparer
        {
            get
            {
                return comparer;
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        private SysTaskRecorderLineServiceContract Model { get; set; }

        /// <summary>
        /// Gets a value indicating whether it is root line for framework/industry map.
        /// </summary>
        /// <value>
        /// <c>true</c> if indicates whether it is root line for framework/industry map; otherwise, <c>false</c>.
        /// </value>
        public bool IsFrameworkIndustryLine
        {
            get
            {
                return this.SyncId == Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the child framework lines.
        /// </summary>
        /// <value>
        /// The child framework lines.
        /// </value>
        public ObservableCollection<FrameworkLineViewModel> ChildLines
        {
            get
            {
                if (this.children == null)
                {
                    this.children = new ObservableCollection<FrameworkLineViewModel>();
                    if (!this.IsFrameworkIndustryLine)
                    {
                        var models = serviceManager.GetImmediateChildFrameworkLines(this.SyncId);
                        models.ForEach(model => this.children.Add(new FrameworkLineViewModel(model, this)));
                    }
                }
                return children;
            }
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return GetName();
            }
            set
            {
                if ((this.Model != null) && (this.Model.Name != value))
                {
                    this.Model.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.Description;
                }
                return null;
            }
            set
            {
                if ((this.Model != null) && (this.Model.Description != value))
                {
                    this.Model.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this line has recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this line has recording; otherwise, <c>false</c>.
        /// </value>
        public bool HasRecording
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.HasRecording;
                }
                return default(bool);
            }
            set
            {
                if ((this.Model != null) && (this.Model.HasRecording != value))
                {
                    this.Model.HasRecording = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>
        /// The module.
        /// </value>
        public SysModule Module
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.Module;
                }
                return default(SysModule);
            }
            set
            {
                if ((this.Model != null) && (this.Model.Module != value))
                {
                    this.Model.Module = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the operation group.
        /// </summary>
        /// <value>
        /// The operation group.
        /// </value>
        public SysTaskRecorderOperationGroup OperationGroup
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.OperationGroup;
                }
                return default(SysTaskRecorderOperationGroup);
            }
            set
            {
                if ((this.Model != null) && (this.Model.OperationGroup != value))
                {
                    this.Model.OperationGroup = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        public RecorderLineOrigin Origin
        {
            get
            {
                if (this.origin == null)
                {
                    if (this.Model != null)
                    {
                        RecorderLineOrigin newOrigin;
                        if (Enum.TryParse<RecorderLineOrigin>(this.Model.Origin, out newOrigin))
                        {
                            this.origin = newOrigin;
                            return this.origin.Value;
                        }
                        else
                        {
                            this.origin = default(RecorderLineOrigin);
                        }
                    }
                }
                else
                {
                    return this.origin.Value;
                }
                return default(RecorderLineOrigin);
            }
            set
            {
                if (this.Model != null)
                {
                    this.Model.Origin = value.ToString();
                    this.origin = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the usage profile.
        /// </summary>
        /// <value>
        /// The usage profile.
        /// </value>
        public SysTaskRecorderUsageProfile UsageProfile
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.UsageProfile;
                }
                return default(SysTaskRecorderUsageProfile);
            }
            set
            {
                if ((this.Model != null) && (this.Model.UsageProfile != value))
                {
                    this.Model.UsageProfile = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the synchronize identifier.
        /// </summary>
        /// <value>
        /// The synchronize identifier.
        /// </value>
        public Guid SyncId
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.SyncId;
                }
                return default(Guid);
            }
            set
            {
                if ((this.Model != null) && (this.Model.SyncId != value))
                {
                    this.Model.SyncId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether to allow artifacts/recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this line allows artifacts/recording; otherwise, <c>false</c>.
        /// </value>
        public bool AllowArtifacts
        {
            get
            {
                return allowArtifacts;
            }
            private set
            {
                if (allowArtifacts != value)
                {
                    allowArtifacts = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether it allows child nodes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this line allows child nodes; otherwise, <c>false</c>.
        /// </value>
        public bool AllowChildNodes
        {
            get
            {
                return allowChildNodes;
            }
            set
            {
                if (allowChildNodes != value)
                {
                    allowChildNodes = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the indentation level.
        /// </summary>
        /// <value>
        /// The indentation level.
        /// </value>        
        private int IndentationLevel
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.IndentationLevel;
                }
                return default(int);
            }
            /* For future use
            set
            {
                if ((this.Model != null) && (this.Model.IndentationLevel != value))
                {
                    this.Model.IndentationLevel = value;
                    OnPropertyChanged();
                }
            }
            */
        }

        /// <summary>
        /// Gets a value indicating whether this line is for basic recording.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this line is for  basic recording; otherwise, <c>false</c>.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Used for data binding")]
        public bool IsBasic
        {
            get
            {
                if (map != null)
                {
                    return !map.GenerateProcessData;
                }
                return default(bool);
            }
        }

        /// <summary>
        /// Gets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.Sequence;
                }
                return default(int);
            }
            /* For future use
            private set
            {
                if ((this.Model != null) && (this.Model.Sequence != value))
                {
                    this.Model.Sequence = value;
                    OnPropertyChanged();
                }
            }
            */
        }

        #endregion

        #region public methods

        /// <summary>
        /// Deletes this framework line and its children.
        /// </summary>
        /// <returns>The parent framework line.</returns>
        public FrameworkLineViewModel Delete()
        {
            var returnLine = this.parent;

            if (this.parent != null)
            {
                this.parent.ChildLines.Remove(this);
                this.parent = null;
            }

            return returnLine;
        }

        /// <summary>
        /// Adds the child to the current framework line.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="map">The framework/industry map.</param>
        /// <exception cref="System.ArgumentNullException">model</exception>
        /// <exception cref="System.ArgumentException">model's parentid does not match with the current line</exception>
        public void AddChild(SysTaskRecorderLineServiceContract model, SysTaskRecorderMapServiceContract map)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (model.ParentId != this.SyncId)
            {
                throw new ArgumentException("Line's parentid does not match with the current line");
            }

            InitializeAllowLevelArtifacts(map);
            this.ChildLines.Add(new FrameworkLineViewModel(model, this));
        }

        #endregion

        #region private methods

        private static void InitializeAllowLevelArtifacts(SysTaskRecorderMapServiceContract map)
        {
            if ((allowLevelArtifacts == null) && (map.FrameworkIndustryLevels != null))
            {
                // Group all falgs per level, whether it allows artifacts at that particular level or not
                allowLevelArtifacts = map.FrameworkIndustryLevels.Select(level => new { level.Sequence, level.AllowArtifacts }).ToDictionary(item => item.Sequence, item => item.AllowArtifacts);
                maxAllowedLevel = allowLevelArtifacts.Keys.Max();
            }
        }

        private void InitializeFlag()
        {
            if (allowLevelArtifacts != null)
            {
                bool isArtfictsEnabled = false;
                allowLevelArtifacts.TryGetValue(this.IndentationLevel, out isArtfictsEnabled);
                this.AllowArtifacts = isArtfictsEnabled;

                this.AllowChildNodes = this.IndentationLevel < maxAllowedLevel;
            }
        }

        private void AddChildLines(Guid currentLineId)
        {
            groupedLines.ExecuteIfFound(currentLineId, childLines =>
            {
                childLines.ForEach(line =>
                {
                    // This condition should be changed when advanced mode recording feature is implemented
                    //if (!(map.GenerateProcessData) && (line.Origin.Equals(RecorderLineOrigin.POS.ToString(), StringComparison.OrdinalIgnoreCase)))
                    {
                        this.ChildLines.Add(new FrameworkLineViewModel(line, this));
                    }
                });
            });
        }

        private string GetName()
        {
            if (this.Model != null)
            {
                return this.Model.Name;
            }
            else
            {
                return this.frameworkIndustryName;
            }
        }
        #endregion

    }

    /// <summary>
    /// Compares two framework line view models using Sequence
    /// </summary>
    public class FrameworkLineViewModelComparer : IComparer<FrameworkLineViewModel>
    {
        /// <summary>
        /// Compares the specified first framework line with second using their Sequence.
        /// </summary>
        /// <param name="x">The first framework line.</param>
        /// <param name="y">The second framework line.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        /// <exception cref="System.ArgumentException">x
        /// or
        /// y</exception>
        public int Compare(FrameworkLineViewModel x, FrameworkLineViewModel y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            return x.Sequence - y.Sequence;
        }
    }

}
