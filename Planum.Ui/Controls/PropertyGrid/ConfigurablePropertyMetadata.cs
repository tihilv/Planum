using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Language.Api;
using Language.Api.Operations;
using Language.Api.Semantic;

namespace Planum.Ui.Controls
{
    public class ConfigurablePropertyMetadata: INotifyPropertyChanged
    {
        private readonly IDocumentModel _documentModel;
        private readonly IPropertyOperation _operation;
        private readonly ISemanticElement[] _hostObjects;
        private Object _value;

        private bool _changed;
        
        public object? ValueAccessor
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    try
                    {
                        _value = value;
                        _changed = true;

                        OnPropertyChanged();
                        OnPropertyChanged(); // <-- Call this twice because otherwise 'TextAndHexadecimal" edit control works not correctly
                    }
                    catch (Exception e)
                    {
                        // this.SetError(nameof(this.ValueAccessor), e.Message); //todo: restore
                        OnPropertyChanged();
                    }
                }
            }
        }

        public string CategoryName { get; }

        public string PropertyName { get; }

        public string PropertyDisplayName { get; }

        public PropertyValueType ValueType { get; }

        public bool IsReadOnly { get; }

        public string Description { get; }

        public static readonly Object Differs = new Object();
        internal ConfigurablePropertyMetadata(IDocumentModel documentModel, IPropertyOperation operation, IEnumerable<ISemanticElement> elements)
        {
            _documentModel = documentModel;
            _operation = operation;
            _hostObjects = elements.ToArray();

            CategoryName = "Common";
            PropertyName = operation.Definition.Text;
            PropertyDisplayName = operation.Definition.Text;
            IsReadOnly = false;
            Description = string.Empty;
            
            var values = elements.Select(o => operation.GetValue(o)).Distinct().ToArray();
            if (values.Length > 1)
                _value = Differs;
            else
                _value = values[0];

            var propertyType = operation.PropertyType;
            if (propertyType == typeof(bool))
            {
                ValueType = PropertyValueType.Bool;
            }
            else if (propertyType == typeof(string) || propertyType == typeof(char) ||
                    propertyType == typeof(double) || propertyType == typeof(float) || propertyType == typeof(decimal) ||
                    propertyType == typeof(int) || propertyType == typeof(uint) ||
                    propertyType == typeof(byte) ||
                    propertyType == typeof(short) || propertyType == typeof(ushort) ||
                    propertyType == typeof(long) || propertyType == typeof(ulong))
            {
                ValueType = PropertyValueType.String;
            }
            else if (propertyType.IsSubclassOf(typeof(Enum)))
            {
                ValueType = PropertyValueType.Enum;
            }
            else
            {
                ValueType = PropertyValueType.Unsupported;
            }
        }

        public override string ToString()
        {
            return $"{CategoryName} - {PropertyDisplayName} (type {ValueType})";
        }

        public Array GetEnumMembers()
        {
            if (ValueType != PropertyValueType.Enum) { throw new InvalidOperationException($"Method {nameof(GetEnumMembers)} not supported on value type {ValueType}!"); }

            return null;//Enum.GetValues(_descriptor.PropertyType);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] String? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CommitValue()
        {
            if (_changed)
            {
                _documentModel.ExecuteOperation(_operation, _hostObjects, new[] { _value });
                _changed = false;
            }
        }
    }
}