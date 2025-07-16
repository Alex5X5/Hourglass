namespace DatabaseUtil.Generator;


public class ColumnInformation(string _OwnerTableName, string _ColumnName, string _Type) {

    string OwnerTableName = _OwnerTableName;
    string ColumnName = _ColumnName;
    string Type = _Type;
    string? Schema = null;
    bool? Nullable = false;
    bool? rowVersion = false;
    int? MaxLength = null;
    int? FixedLength = null;
    bool? IsPrimaryKey = false;

    public ColumnInformation(
        string _OwnerTableName,
        string _ColumnName,
        string _Type,
        bool? _isPrimaryKey = null,
        bool? _Nullable = null,
        int? _maxLength = null,
        int? _fixedLength = null,
        bool? _rowVersion = null,
        string? _schema = null
    ) : this(_OwnerTableName, _ColumnName, _Type) {
        if(_Type!= null )
            Type = (string)_Type;
        if(_isPrimaryKey!=null)
            IsPrimaryKey = (bool)_isPrimaryKey;
        if(_Nullable != null )
            Nullable = (bool)_Nullable;
        if(_rowVersion != null )
            rowVersion = (bool)_rowVersion;
        if(_schema!=null)
            Schema = (string)_schema;
        if(_maxLength!=null)
            MaxLength = _maxLength;
        if(_fixedLength!=null)
            FixedLength = _fixedLength;
    }

    public object AsAnnoymusObject() {
        return new { };
    }

    public ColumnInformation SetType(string value) {
        Type = value;
        return this;
    }
    
    public ColumnInformation SetIsPrimaryKey(bool value) {
        IsPrimaryKey = value;
        return this;
    }

    public ColumnInformation SetNullable(bool value) {
        Nullable = value;
        return this;
    }

    public ColumnInformation SetMaxLength(int value) {
        MaxLength = value;
        return this;
    }

    public ColumnInformation SetFixedLength(int value) {
        FixedLength = value;
        return this;
    }

    public ColumnInformation SetSchema(string value) {
        Schema = value;
        return this;
    }


}
