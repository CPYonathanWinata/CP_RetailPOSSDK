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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSRetailPosis;
using Microsoft.Dynamics.Retail.Pos.TaskRecorder.Common;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Controls
{
    /// <summary>
    /// Bindable tree view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DefaultEvent("SelectedItemChanged")]
    public class BindableTreeView<T> : TreeView where T : INotifyPropertyChanged
    {
        #region property backend fields

        private ObservableCollection<T> dataSource;
        private Func<T, string> displayMember;
        private Func<T, ObservableCollection<T>> hierarchyMember;
        private Func<T, bool> altStyleNodeMember;
        private Func<T, bool> topLevelNodeStyleMember;
        private T selectedItem;
        private IComparer<T> itemSorter;

        #endregion

        #region private fields

        private Dictionary<T, BindableTreeViewNode<T>> nodes;

        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableTreeView{T}"/> class.
        /// </summary>
        public BindableTreeView()
        {
            this.AfterSelect += BindableTreeView_AfterSelect;
            this.BeforeExpand += BindableTreeView_BeforeExpand;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "This is the data source, we need to be able to set it")]
        public ObservableCollection<T> DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                if (this.dataSource != value)
                {
                    if (this.dataSource != null)
                    {
                        this.dataSource.CollectionChanged -= DataSource_CollectionChanged;
                        foreach (var item in this.dataSource)
                        {
                            UnsubscribeCollectionChangeHandlers(item);
                        }
                    }

                    dataSource = value;

                    if (this.dataSource != null)
                    {
                        this.dataSource.CollectionChanged += DataSource_CollectionChanged;
                    }

                    this.ResetNodes();
                }
            }
        }

        /// <summary>
        /// Gets or sets the display member.
        /// </summary>
        /// <value>
        /// The display member.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It wants to avoid nested generics because it thinks that using it will be very complex. But here the Expression is created automatically and the usage is very simple. For example: 'vm => vm.Name'")]
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
        }

        /// <summary>
        /// Gets or sets the hierarchy member.
        /// </summary>
        /// <value>
        /// The hierarchy member.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It wants to avoid nested generics because it thinks that using it will be very complex. But here the Expression is created automatically and the usage is very simple. For example: 'person => person.Children'")]
        public Func<T, ObservableCollection<T>> HierarchyMember
        {
            get
            {
                return hierarchyMember;
            }
            set
            {
                if (this.hierarchyMember != value)
                {
                    hierarchyMember = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if a node should have the alt style applied (applies alt style image).
        /// </summary>
        /// <value>
        /// The alt style node member.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It wants to avoid nested generics because it thinks that using it will be very complex. But here the Expression is created automatically and the usage is very simple. For example: 'vm => vm.Name'")]
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
        }

        /// <summary>
        /// Gets or sets a value indicating if a node is toplevel node (applies top level style image).
        /// </summary>
        /// <value>
        /// The top level style node member.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "It wants to avoid nested generics because it thinks that using it will be very complex. But here the Expression is created automatically and the usage is very simple. For example: 'vm => vm.Name'")]
        public Func<T, bool> TopLevelStyleNodeMember
        {
            get
            {
                return topLevelNodeStyleMember;
            }
            set
            {
                if (topLevelNodeStyleMember != value)
                {
                    topLevelNodeStyleMember = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public T SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (!object.Equals(this.selectedItem, value))
                {
                    selectedItem = value;

                    if (this.SelectedItemChanged != null)
                    {
                        this.SelectedItemChanged(this, EventArgs.Empty);
                    }

                    this.nodes.ExecuteIfFound(value, node =>
                    {
                        if (this.SelectedNode != node)
                        {
                            this.SelectedNode = node;
                            this.Select();
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Gets or sets the item sorter.
        /// </summary>
        /// <value>
        /// The item sorter.
        /// </value>
        public IComparer<T> ItemSorter
        {
            get
            {
                return itemSorter;
            }
            set
            {
                itemSorter = value;
                if (this.itemSorter != null)
                {
                    try
                    {
                        this.TreeViewNodeSorter = new BindableTreeNodeComparer<T>(this.itemSorter);
                    }
                    catch (Exception ex)
                    {
                        ApplicationExceptionHandler.HandleException(this.ToString(), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when selected item is changed.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region private methods

        private void BindableTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BindableTreeViewNode<T> selectedNode = this.SelectedNode as BindableTreeViewNode<T>;
            if (selectedNode != null)
            {
                this.SelectedItem = selectedNode.DataContext;
            }

        }

        private void DataSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.SelectedNode == null)
            {
                return;
            }

            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null)
                        {
                            BindableTreeViewNode<T> firstNode = this.SelectedNode.FirstNode as BindableTreeViewNode<T>;
                            if ((firstNode != null) && (firstNode.IsEmptyNode))
                            {
                                this.SelectedNode.Nodes.Clear();
                            }

                            foreach (var item in e.NewItems.OfType<T>())
                            {
                                ObservableCollection<T> children = this.HierarchyMember(item);
                                if (children != null)
                                {
                                    children.CollectionChanged += DataSource_CollectionChanged;
                                }
                                this.CreateNode(item, this.SelectedNode);
                            }
                            this.Select();
                        }
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems.OfType<T>())
                            {
                                this.UnsubscribeCollectionChangeHandlers(item);
                                this.RemoveNode(item);
                            }
                            this.Select();
                        }
                        break;
                    }
            }
        }

        private void RemoveNode(T data)
        {
            this.nodes.ExecuteIfFound(data, node =>
            {
                if (node.Parent != null)
                {
                    node.Parent.Nodes.Remove(node);
                }
                else
                {
                    node.TreeView.Nodes.Remove(node);
                }

                this.nodes.Remove(data);
            });
        }

        private void UnsubscribeCollectionChangeHandlers(T item)
        {
            this.nodes.ExecuteIfFound(item, node =>
                {
                    Queue<BindableTreeViewNode<T>> currentNodes = new Queue<BindableTreeViewNode<T>>();
                    currentNodes.Enqueue(node);

                    while (currentNodes.Any())
                    {
                        var currentParentNode = currentNodes.Dequeue();
                        var firstNode = currentParentNode.FirstNode as BindableTreeViewNode<T>;
                        if ((firstNode != null) && (!firstNode.IsEmptyNode))
                        {
                            var currentChildItems = this.HierarchyMember(currentParentNode.DataContext);
                            if (currentChildItems != null)
                            {
                                currentChildItems.CollectionChanged -= DataSource_CollectionChanged;
                            }

                            var children = currentParentNode.Nodes;
                            if (children != null)
                            {
                                foreach (BindableTreeViewNode<T> dataItem in children)
                                {
                                    currentNodes.Enqueue(dataItem);
                                }
                            }
                        }
                    }
                });
        }

        private void ResetNodes()
        {
            this.Nodes.Clear();
            nodes = new Dictionary<T, BindableTreeViewNode<T>>();
            foreach (var item in this.DataSource)
            {
                this.CreateParentNode(item, this);
            }

            this.ExpandTopLevelNodes();
        }

        private void BindableTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                BindableTreeViewNode<T> node = e.Node as BindableTreeViewNode<T>;
                if (node != null)
                {
                    BindableTreeViewNode<T> firstNode = node.FirstNode as BindableTreeViewNode<T>;
                    if ((firstNode != null) && (firstNode.IsEmptyNode))
                    {
                        e.Node.Nodes.Clear();
                        var childItems = this.HierarchyMember(node.DataContext);
                        if (childItems != null)
                        {
                            childItems.CollectionChanged += DataSource_CollectionChanged;
                            foreach (var childItem in childItems)
                            {
                                this.CreateNode(childItem, e.Node);
                            }
                        }
                    }
                }
            }
        }

        private void CreateNode(T data, TreeNode parent)
        {
            if (!this.nodes.ContainsKey(data))
            {
                var node = new BindableTreeViewNode<T>() { DisplayMember = this.DisplayMember, AltStyleNodeMember = this.AltStyleNodeMember, DataContext = data };
                node.Nodes.Add(BindableTreeViewNode<T>.NewEmptyNode());
                if (parent != null)
                {
                    parent.Nodes.Add(node);
                    this.nodes.Add(data, node);
                }

                node.Text = this.DisplayMember(data);
                node.IsAltStyle = this.AltStyleNodeMember(data); 
            }
        }

        private void CreateParentNode(T data, TreeView parent)
        {
            if (!this.nodes.ContainsKey(data))
            {
                var node = new BindableTreeViewNode<T>() { DisplayMember = this.DisplayMember, AltStyleNodeMember = this.AltStyleNodeMember, DataContext = data };
                node.Nodes.Add(BindableTreeViewNode<T>.NewEmptyNode());
                if (parent != null)
                {
                    parent.Nodes.Add(node);
                    this.nodes.Add(data, node);
                }

                node.Text = this.DisplayMember(data);
                node.IsAltStyle = this.AltStyleNodeMember(data); 
            }
        }

        private void ExpandTopLevelNodes()
        {
            foreach (TreeNode node in this.Nodes)
            {
                node.Expand();
            }
        }
    }
        #endregion
}
