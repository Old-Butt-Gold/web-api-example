using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Entities.Models;

public class Entity : DynamicObject, IXmlSerializable, IDictionary<string, object>
{
    private const string RootElement = "Entity";
    private const string TypeAttributeName = "type";
    private readonly Dictionary<string, object> _properties = [];

    #region DynamicObject Overrides

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return _properties.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _properties[binder.Name] = value;
        return true;
    }

    #endregion

    #region IXmlSerializable Implementation

    public XmlSchema GetSchema() => null;

    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement(RootElement);

        while (!reader.Name.Equals(RootElement))
        {
            var name = reader.Name;

            reader.MoveToAttribute(TypeAttributeName);
            var typeContent = reader.ReadContentAsString();
            var underlyingType = Type.GetType(typeContent);
            reader.MoveToContent();
            
            _properties[name] = reader.
                ReadElementContentAs(underlyingType, null);
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var (key, value) in _properties)
        {
            writer.WriteStartElement(key);
            writer.WriteString(value.ToString());
            writer.WriteEndElement();
        }
    }

    #endregion

    #region IDictionary Implementation

    public object this[string key]
    {
        get => _properties[key];
        set => _properties[key] = value;
    }

    public ICollection<string> Keys => _properties.Keys;
    public ICollection<object> Values => _properties.Values;
    public int Count => _properties.Count;
    public bool IsReadOnly => false;

    public void Add(string key, object value) => _properties.Add(key, value);
    public bool ContainsKey(string key) => _properties.ContainsKey(key);
    public bool Remove(string key) => _properties.Remove(key);
    public bool TryGetValue(string key, out object value) => _properties.TryGetValue(key, out value);
    public void Clear() => _properties.Clear();
    public void Add(KeyValuePair<string, object> item) => _properties.Add(item.Key, item.Value);
    public bool Contains(KeyValuePair<string, object> item) => _properties.Contains(item);
    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotImplementedException();
    public bool Remove(KeyValuePair<string, object> item) => _properties.Remove(item.Key);
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _properties.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Helpers

    private static Type GetType(string typeFullName)
    {
        return Type.GetType(typeFullName) 
            ?? throw new InvalidOperationException($"Type '{typeFullName}' not found");
    }

    #endregion
}