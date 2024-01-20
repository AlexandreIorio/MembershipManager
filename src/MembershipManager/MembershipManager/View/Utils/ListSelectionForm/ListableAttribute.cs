namespace MembershipManager.View.Utils.ListSelectionForm
{

    ///These attributes are used to define the behaviour of the list selection form

    /// <summary>
    /// This attribute is used to define that the list can be filtered by this property
    /// </summary>
    /// <param name="friendlyName"> The name to display in the filter selector </param>
    /// <param name="isDefault"> Set the default filter with this property </param>

    internal class Filtered(string friendlyName, bool isDefault = false) : Attribute
    {
        public bool IsDefault { get; set; } = isDefault;
        public string FriendlyName { get; set; } = friendlyName;

    }

    /// <summary>
    /// This attribute is used to define that the list can be display this property
    /// </summary>
    /// <param name="headerName">The name to display in the header</param>
    internal class Displayed(string headerName) : Attribute
    {
        public string HeaderName { get; set; } = headerName;
    }

    /// <summary>
    /// This attribute is used to define that the list is default sorted by this property
    /// </summary>
    internal class Sorted() : Attribute { }

    /// <summary>
    /// This attribute is used to define that the list can be display this property
    /// </summary>
    /// <param name="headerName">The name to display in the header</param>
    internal class TextFormat(string format) : Attribute
    {
        public string Format { get; set; } = format;
    }


}
