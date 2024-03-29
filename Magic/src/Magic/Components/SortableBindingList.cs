using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Library.Forms
{
    /// <summary>
    /// Provides a generic collection that supports data binding and additionally supports sorting.
    /// See http://msdn.microsoft.com/en-us/library/ms993236.aspx
    /// If the elements are IComparable it uses that; otherwise compares the ToString()
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SortableBindingList<T> : BindingList<T> where T : class
    {
        private Boolean _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        public SortableBindingList()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        /// <param name="list">An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the <see cref="T:System.ComponentModel.BindingList`1" />.</param>
        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override Boolean SupportsSortingCore
        {
            get { return true; }
        }
        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override Boolean IsSortedCore
        {
            get { return this._isSorted; }
        }

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return this._sortDirection; }
        }
        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this._sortProperty; }
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            this._sortProperty = prop;
            this._sortDirection = direction;

            List<T> list = this.Items as List<T>;
            if (list == null) return;

            list.Sort(this.Compare);

            this._isSorted = true;
            //fire an event that the list has been changed.
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        /// <summary>
        /// Removes any sort applied with ApplySortCore if sorting is implemented
        /// </summary>
        protected override void RemoveSortCore()
        {
            this._sortDirection = ListSortDirection.Ascending;
            this._sortProperty = null;
            this._isSorted = false; //thanks Luca
        }


        private Int32 Compare(T lhs, T rhs)
        {
            var result = this.OnComparison(lhs, rhs);
            //invert if descending
            if (this._sortDirection == ListSortDirection.Descending)
                result = -result;
            return result;
        }

        private Int32 OnComparison(T lhs, T rhs)
        {
            Object lhsValue = lhs == null ? null : this._sortProperty.GetValue(lhs);
            Object rhsValue = rhs == null ? null : this._sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1; //nulls are equal
            }
            if (rhsValue == null)
            {
                return 1; //first has value, second doesn't
            }
            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }
            if (lhsValue.Equals(rhsValue))
            {
                return 0; //both are the same
            }
            //not comparable, compare ToString
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}