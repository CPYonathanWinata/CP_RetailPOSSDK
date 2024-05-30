/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Localization;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.MVVM;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Services;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.SysTaskRecorder;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.ViewModels
{
    /// <summary>
    /// View model for new node creation
    /// </summary>
    public class NewNodeViewModel : NotifyPropertyChangedBase
    {
        #region property backend fields

        private string name;
        private string description;
        private ReadOnlyCollection<SysModule> availableModules;
        private SysModule selectedModule;
        private ReadOnlyCollection<SysTaskRecorderOperationGroup> availableOperationGroups;
        private SysTaskRecorderOperationGroup selectedOperationGroup;

        #endregion

        #region private fields

        private string nodeFrameworkId;
        private string nodeIndustryCode;
        private Guid nodeParentId;
        private TaskRecorderService serviceManager = TaskRecorderService.Instance;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewNodeViewModel"/> class.
        /// </summary>
        /// <param name="parentNodeId">The parent node identifier.</param>
        /// <param name="frameworkId">The framework identifier.</param>
        /// <param name="industryCode">The industry code.</param>
        public NewNodeViewModel(FrameworkLineViewModel parentNode, string frameworkId, string industryCode)
        {
            if (parentNode != null)
            {
                this.nodeParentId = parentNode.SyncId;
                InitializeNewNodeValues(parentNode);
            }

            this.nodeFrameworkId = frameworkId;
            this.nodeIndustryCode = industryCode;

            this.AvailableModules = new ReadOnlyCollection<SysModule>(Utils.GetEnumValues<SysModule>());
            this.AvailableOperationGroups = new ReadOnlyCollection<SysTaskRecorderOperationGroup>(Utils.GetEnumValues<SysTaskRecorderOperationGroup>());
        }

        #endregion

        #region properties

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
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                    OnPropertyChanged("CanSaveNode");
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
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the available modules.
        /// </summary>
        /// <value>
        /// The available modules.
        /// </value>
        public ReadOnlyCollection<SysModule> AvailableModules
        {
            get
            {
                return availableModules;
            }
            private set
            {
                if (availableModules != value)
                {
                    availableModules = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the localized available modules.
        /// </summary>
        /// <value>
        /// The localized available modules.
        /// </value>
        public Collection<LocalizedSysModule> LocalizedAvailableModules
        {
            get
            {
                if (localizedAvailableModules == null)
                {
                    localizedAvailableModules = EnumLocalizedValueConverter.GetLocalizedEnumValues<LocalizedSysModule, SysModule>(AvailableModules);
                }
                return localizedAvailableModules;
            }
        }private Collection<LocalizedSysModule> localizedAvailableModules;

        /// <summary>
        /// Gets or sets the selected module.
        /// </summary>
        /// <value>
        /// The selected module.
        /// </value>
        public SysModule SelectedModule
        {
            get
            {
                return selectedModule;
            }
            set
            {
                if (selectedModule != value)
                {
                    selectedModule = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the available operation groups.
        /// </summary>
        /// <value>
        /// The available operation groups.
        /// </value>
        public ReadOnlyCollection<SysTaskRecorderOperationGroup> AvailableOperationGroups
        {
            get
            {
                return availableOperationGroups;
            }
            private set
            {
                if (availableOperationGroups != value)
                {
                    availableOperationGroups = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the localized available operation groups.
        /// </summary>
        /// <value>
        /// The localzied available operation groups.
        /// </value>
        public Collection<LocalizedSysTaskRecorderOperationGroup> LocalizedAvailableOperationGroups
        {
            get
            {
                if (localizedAvailableOperationGroups == null)
                {
                    localizedAvailableOperationGroups = EnumLocalizedValueConverter.GetLocalizedEnumValues<LocalizedSysTaskRecorderOperationGroup, SysTaskRecorderOperationGroup>(AvailableOperationGroups); ;
                }
                return localizedAvailableOperationGroups;
            }
        }private Collection<LocalizedSysTaskRecorderOperationGroup> localizedAvailableOperationGroups;


        /// <summary>
        /// Gets or sets the selected operation group.
        /// </summary>
        /// <value>
        /// The selected operation group.
        /// </value>
        public SysTaskRecorderOperationGroup SelectedOperationGroup
        {
            get
            {
                return selectedOperationGroup;
            }
            set
            {
                if (selectedOperationGroup != value)
                {
                    selectedOperationGroup = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the created line.
        /// </summary>
        /// <value>
        /// The created line.
        /// </value>
        public SysTaskRecorderLineServiceContract CreatedLine { get; private set; }

        #endregion

        #region SaveNodeCommand

        public void ExecuteSaveNode()
        {
            this.CreatedLine = this.serviceManager.CreateFrameworkLine(this.Name, this.Description, this.SelectedModule, this.SelectedOperationGroup, this.nodeParentId, this.nodeFrameworkId, this.nodeIndustryCode);
        }

        public bool CanSaveNode
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(this.Name));
            }
        }

        #endregion

        #region private methods

        private void InitializeNewNodeValues(FrameworkLineViewModel parentNode)
        {
            if (!parentNode.IsFrameworkIndustryLine)
            {
                this.Name = parentNode.Name;
                this.Description = parentNode.Description;
                this.SelectedModule = parentNode.Module;
                this.SelectedOperationGroup = parentNode.OperationGroup;
            }
        }

        #endregion

    }
}
