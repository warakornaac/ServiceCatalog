namespace ServiceCatalog.Library
{
    public class ColumnNameAttribute : System.Attribute
    {
        private readonly string _key = string.Empty;

        public string Key
        {
            get { return this._key; }
        }

        public ColumnNameAttribute(string key)
        {
            this._key = key;
        }
    }
}
