using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Treeview.Models
{
    public class TreeViewModel:BaseViewModel
    {

        //TODO: (Comments By Vishesh ) variable name are not correct according to our codeing standard.
        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        /// <value>
        /// The node unique Id.
        /// </value>
        public long NodeId { get; set; }
        /// <summary>
        /// Gets or sets the parent Id
        /// </summary>
        /// <value>
        /// The parent Id.
        /// </value>
        public long? ParentId { get; set; }
        /// <summary>
        /// Gets or sets the node text.
        /// </summary>
        /// <value>
        /// The node text.
        /// </value>
        public string NodeText{ get; set; }
        /// <summary>
        /// Gets or sets the list of items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        // ReSharper disable once InconsistentNaming
        // As we are using this property in Kendo Treeview. Its case sensitive and only allows 'items' as Property name.
        public List<TreeViewModel> items { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [expanded].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [expanded]; otherwise, <c>false</c>.
        /// </value>
        // As we are using this property in Kendo Treeview. Its case sensitive and only allows 'expanded' as Property name.
        // ReSharper disable once InconsistentNaming
        public bool expanded { get; set; }
        /// <summary>
        /// Gets or sets the sprite CSS class.
        /// </summary>
        /// <value>
        /// The sprite CSS class.
        /// </value>
        // As we are using this property in Kendo Treeview. Its case sensitive and only allows 'spriteCssClass' as Property name.
        // ReSharper disable once InconsistentNaming
        public string spriteCssClass { get; set; }
        /// <summary>
        /// Gets or sets the append string.
        /// </summary>
        /// <value>
        /// The append string.
        /// </value>
        public string AppendString { get; set; }
        /// <summary>
        /// Gets or sets the is contract.
        /// </summary>
        /// <value>
        /// The is contract.
        /// </value>
        public bool? IsContract { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public long ContractId { get; set; }
        /// <summary>
        /// Gets or sets the contract service type Id.
        /// </summary>
        /// <value>
        /// The contract service type unique Id.
        /// </value>
        public long ContractServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is primary node].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is primary node]; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryNode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is primary model contract].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is primary model contract]; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryModelContract { get; set; }

        public string RenameText { get; set; }

        public  bool IsExpanded { get; set; }
        /// <summary>
        /// Set or Get CarveOut.
        /// </summary>
        /// <value>
        /// The CarveOut.
        /// </value>
        public bool IsCarveOut { get; set; }

        /// <summary>
        /// Gets or sets the node display text.
        /// </summary>
        /// <value>
        /// The node display text.
        /// </value>
        public string NodeDisplayText { get; set; }

       
    }
}
