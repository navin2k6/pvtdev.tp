using System;
using System.Data;
using Debug = System.Diagnostics.Debug;

// This dsupportive class used to generate a sql command.
// Note: Please do not change.
namespace AseAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AseCommandAttribute : Attribute
    {

        private string _commandText;
        private CommandType _commandType;

        public AseCommandAttribute(CommandType commandType)
            :
            this(CommandType.StoredProcedure, null) { }

        public AseCommandAttribute(CommandType commandType, string commandText)
        {
            _commandText = commandText;
            _commandType = commandType;
        }

        public string CommandText
        {
            get { return _commandText == null ? string.Empty : _commandText; }
            set { _commandText = value; }
        }

        public CommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class AseParameterAttribute : Attribute
    {
        private string _name;
        private bool _paramTypeDefined;

        private SqlDbType _paramType;
        private int _size;
        private byte _precision;
        private byte _scale;
        private bool _directionDefined;
        private ParameterDirection _direction;

        public AseParameterAttribute() { }

        public AseParameterAttribute(string name)
        {
            Name = name;
        }

        public AseParameterAttribute(int size)
        {
            Size = size;
        }

        public AseParameterAttribute(SqlDbType paramType)
        {
            AseType = paramType;
        }

        public AseParameterAttribute(string name, SqlDbType paramType)
        {
            Name = name;
            AseType = paramType;
        }

        public AseParameterAttribute(SqlDbType paramType, int size)
        {
            AseType = paramType;
            Size = size;
        }

        public AseParameterAttribute(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public AseParameterAttribute(string name, SqlDbType paramType, int size)
        {
            Name = name;
            AseType = paramType;
            Size = size;
        }

        public AseParameterAttribute(ParameterDirection direction)
        {
            Direction = direction;
        }

        public string Name
        {
            get { return _name == null ? string.Empty : _name; }
            set { _name = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public byte Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        public byte Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public ParameterDirection Direction
        {
            get
            {
                Debug.Assert(_directionDefined);
                return _direction;
            }

            set
            {
                _direction = value;
                _directionDefined = true;
            }
        }

        public SqlDbType AseType
        {
            get
            {
                Debug.Assert(_paramTypeDefined);
                return _paramType;
            }

            set
            {
                _paramType = value;
                _paramTypeDefined = true;
            }
        }

        public bool IsNameDefined
        {
            get { return _name != null && _name.Length != 0; }
        }

        public bool IsSizeDefined
        {
            get { return _size != 0; }
        }

        public bool IsTypeDefined
        {
            get { return _paramTypeDefined; }
        }

        public bool IsDirectionDefined
        {
            get { return _directionDefined; }
        }

        public bool IsScaleDefined
        {
            get { return _scale != 0; }
        }

        public bool IsPrecisionDefined
        {
            get { return _precision != 0; }
        }
    }
}

namespace AseAttributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class NonCommandParameterAttribute : Attribute
    {
    }
}