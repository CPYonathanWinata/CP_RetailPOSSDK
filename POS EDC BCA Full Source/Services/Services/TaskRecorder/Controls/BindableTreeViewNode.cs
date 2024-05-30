/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Controls
{
    /// <summary>
    /// Node for BindableTreeView
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Justification = "We never serialize this tree node")]
    public class BindableTreeViewNode<T> : TreeNode where T : INotifyPropertyChanged
    {
        #region constructors

        /// <summary>
        /// Initializes the <see cref="BindableTreeViewNode{T}"/> class.
        /// </summary>
        public BindableTreeViewNode()
        {
            this.IsEmptyNode = false;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        public T DataContext
        {
            get
            {
                return dataContext;
            }
            set
            {
                if (dataContext != null)
                {
                    dataContext.PropertyChanged -= DataContext_PropertyChanged;
                }
                dataContext = value;
                if (dataContext != null)
                {
                    dataContext.PropertyChanged += DataContext_PropertyChanged;
                }
            }
        }private T dataContext;

        /// <summary>
        /// Gets or sets the display member.
        /// </summary>
        /// <value>
        /// The display member.
        /// </value>
        public Func<T, string> DisplayMember
        {
            get
            {
                return displayMember;
            }
            set
            {
                if (this.displayMember != value)
                {
                    displayMember = value;
                }
            }
        }private Func<T, string> displayMember;

        /// <summary>
        /// Gets or sets the alt style node member.
        /// </summary>
        /// <value>
        /// The alt style node member.
        /// </value>
        public Func<T, bool> AltStyleNodeMember
        {
            get
            {
                return altStyleNodeMember;
            }
            set
            {
                if (this.altStyleNodeMember != value)
                {
                    altStyleNodeMember = value;
                }
            }
        }private Func<T, bool> altStyleNodeMember;

        /// <summary>
        /// Gets or sets a value indicating whether alt style should be applied to this node.
        /// </summary>
        /// <value>
        ///   <c>true</c> if alt style should be applied; otherwise, <c>false</c>.
        /// </value>
        public bool IsAltStyle
        {
            get
            {
                return isAltStyle;
            }
            set
            {
                if (this.isAltStyle != value)
                {
                    isAltStyle = value;

                    if (this.isAltStyle)
                    {
                        this.ImageKey = BindableTreeViewImagesKey.AltStyleNodeImage.ToString();
                    }
                    else
                    {
                        this.ImageKey = BindableTreeViewImagesKey.NormalNodeImage.ToString();
                    }

                    this.SelectedImageKey = this.ImageKey;
                }
            }
        }private bool isAltStyle;

        #region Empty node

        /// <summary>
        /// Gets a value indicating whether this instance is an empty node.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an empty node; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmptyNode { get; private set; }

        /// <summary>
        /// Creates a new empty node.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "We should be able to call this using class and not object. So, not changing into instance member")]
        public static BindableTreeViewNode<T> NewEmptyNode()
        {
            return new EmptyBindableTreeViewNode();
        }

        private class EmptyBindableTreeViewNode : BindableTreeViewNode<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EmptyBindableTreeViewNode"/> class.
            /// </summary>
            public EmptyBindableTreeViewNode()
            {
                this.IsEmptyNode = true;
            }
        }

        #endregion

        #endregion

        #region private methods

        private void DataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.DisplayMember != null)
            {
                this.Text = this.DisplayMember(this.DataContext);
            }
            if (this.AltStyleNodeMember != null)
            {
                this.IsAltStyle = this.AltStyleNodeMember(this.DataContext);
            }
        }

        #endregion
    }
}
