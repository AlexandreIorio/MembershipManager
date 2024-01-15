using NpgsqlTypes;

namespace MembershipManager.Engine
{
    ///These attributes are used to specify the database structure of a class

    #region class Attributes
    /// <summary>
    /// Used to specify the table name of a class
    /// </summary>
    /// <param name="tableName"> the table name</param>
    [AttributeUsage(AttributeTargets.Class)]
    internal class DbTableName(string tableName) : Attribute
    {
        public string Name { get; private set; } = tableName;
    }

    /// <summary>
    /// Used to that a class is a child of another class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class DbInherit : Attribute
    {
        public Type InheritType { get; private set; }

        public DbInherit(Type inheritType)
        {
            this.InheritType = inheritType;
        }
    }

    #endregion

    #region Property Attributes

    /// <summary>
    /// Used to specify that an attribute is a column name
    /// </summary>
    /// <param name="propertyName"></param>
    internal abstract class DbNameable(string propertyName) : Attribute
    {
        public string Name { get; private set; } = propertyName;
    }

    /// <summary>
    /// Used to specify the constraints of a property
    /// </summary>

    [AttributeUsage(AttributeTargets.Property)]
    internal class DbConstraint : Attribute { }
    /// <summary>
    /// Used to specify the primary key of a class
    /// </summary>
    /// <param name="pkType">The sql type of primary key</param>
    /// <param name="size">The size of primary key </param>
    internal class DbPrimaryKey(NpgsqlDbType pkType, int size = 0) : DbConstraint
    {
        public NpgsqlDbType PkType { get; private set; } = pkType;
        public int Size { get; private set; } = size;
    }

    /// <summary>
    /// Used to specify the name of a property
    /// </summary>
    /// <param name="attributeName"></param>
    [AttributeUsage(AttributeTargets.Property)]
    internal class DbAttribute(string attributeName) : DbNameable(attributeName) { }

    /// <summary>
    /// Used to specify the relation between two classes
    /// </summary>

    /// <param name="attributeName">The name of the attribute in the class</param>
    [AttributeUsage(AttributeTargets.Property)]
    internal class DbRelation(string attributeName) : DbNameable(attributeName)
    {

    }
    #endregion



}
