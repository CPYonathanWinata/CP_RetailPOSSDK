/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Dynamics.Retail.Pos.TaskRecorder.Controls
{
    /// <summary>
    /// Compares two bindable tree view nodes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BindableTreeNodeComparer<T> : IComparer where T : INotifyPropertyChanged
    {
        IComparer<T> treeNodeItemComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableTreeNodeComparer{T}"/> class.
        /// </summary>
        /// <param name="itemComparer">The item comparer to be used to compare.</param>
        /// <exception cref="System.ArgumentException">itemComparer</exception>
        public BindableTreeNodeComparer(IComparer<T> itemComparer)
        {
            if (itemComparer == null)
            {
                throw new ArgumentNullException("itemComparer");
            }

            treeNodeItemComparer = itemComparer;
        }

        /// <summary>
        /// Compares the specified first bindable tree node.
        /// </summary>
        /// <param name="x">The first bindable tree view node.</param>
        /// <param name="y">The second bindable tree view node.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        /// <exception cref="System.ArgumentException">x
        /// or
        /// y</exception>
        public int Compare(object x, object y)
        {
            BindableTreeViewNode<T> firstTreeNode = x as BindableTreeViewNode<T>;
            if ((x == null) || (firstTreeNode == null))
            {
                throw new ArgumentNullException("x");
            }

            BindableTreeViewNode<T> secondTreeNode = y as BindableTreeViewNode<T>;
            if ((y == null) || (secondTreeNode == null))
            {
                throw new ArgumentNullException("y");
            }

            // Making empty nodes as comparitively less than non-empty nodes
            int relativeValue = ((firstTreeNode.IsEmptyNode) && (!secondTreeNode.IsEmptyNode)) ? -1 :
                (((!firstTreeNode.IsEmptyNode) && (secondTreeNode.IsEmptyNode)) ? 1 : 0);

            // If both are non-empty nodes, then make the actual comparision
            if (!((firstTreeNode.IsEmptyNode) && (secondTreeNode.IsEmptyNode)))
            {
                relativeValue = this.treeNodeItemComparer.Compare(firstTreeNode.DataContext, secondTreeNode.DataContext);
            }

            return relativeValue;
        }
    }
}
