using Complete;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace RtmpSharp.IO {
  [Serializable]
  [TypeConverter(typeof(AsObjectConverter))]
  public class AsObject : DynamicObject, IDictionary, IDictionary<string, object> {
    readonly Dictionary<string, object> underlying;

    public string TypeName { get; set; }
    public bool IsTyped { get { return !string.IsNullOrEmpty(TypeName); } }

    public AsObject() {
      underlying = new Dictionary<string, object>();
    }

    public AsObject(string typeName) : this() {
      this.TypeName = typeName;
    }

    public AsObject(Dictionary<string, object> dictionary) : this() {
      underlying = new Dictionary<string, object>(dictionary);
    }

    public override IEnumerable<string> GetDynamicMemberNames() {
      return underlying.Keys;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result) {
      return underlying.TryGetValue(binder.Name, out result);
    }

    public override bool TryDeleteMember(DeleteMemberBinder binder) {
      return underlying.Remove(binder.Name);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value) {
      underlying[binder.Name] = value;
      return true;
    }

    #region IDictionary members

    ICollection IDictionary.Keys {
      get {
        return ((IDictionary) this.underlying).Keys;
      }
    }

    ICollection IDictionary.Values {
      get {
        return ((IDictionary) this.underlying).Values;
      }
    }

    public bool IsFixedSize {
      get {
        return ((IDictionary) this.underlying).IsFixedSize;
      }
    }

    public object SyncRoot {
      get {
        return ((IDictionary) this.underlying).SyncRoot;
      }
    }

    public bool IsSynchronized {
      get {
        return ((IDictionary) this.underlying).IsSynchronized;
      }
    }

    public object this[object key] {
      get {
        return ((IDictionary) this.underlying)[key];
      }

      set {
        ((IDictionary) this.underlying)[key] = value;
      }
    }

    public bool Contains(object key) {
      return ((IDictionary) this.underlying).Contains(key);
    }

    public void Add(object key, object value) {
      ((IDictionary) this.underlying).Add(key, value);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator() {
      return ((IDictionary) this.underlying).GetEnumerator();
    }

    public void Remove(object key) {
      ((IDictionary) this.underlying).Remove(key);
    }

    public void CopyTo(Array array, int index) {
      ((IDictionary) this.underlying).CopyTo(array, index);
    }

    #endregion

    #region IDictionary<> members

    public int Count { get { return underlying.Count; } }
    public bool IsReadOnly { get { return ((IDictionary<string, object>) underlying).IsReadOnly; } }
    public ICollection<string> Keys { get { return underlying.Keys; } }
    public ICollection<object> Values { get { return underlying.Values; } }

    public void Add(KeyValuePair<string, object> item) {
      ((IDictionary<string, object>) underlying).Add(item);
    }

    public void Clear() {
      underlying.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item) {
      return underlying.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
      ((IDictionary<string, object>) underlying).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, object> item) {
      return ((IDictionary<string, object>) underlying).Remove(item);
    }

    public void Add(string key, object value) {
      underlying.Add(key, value);
    }

    public bool ContainsKey(string key) {
      return underlying.ContainsKey(key);
    }

    public object this[string key] {
      get { return underlying[key]; }
      set { underlying[key] = value; }
    }

    public bool Remove(string key) {
      return underlying.Remove(key);
    }

    public bool TryGetValue(string key, out object value) {
      return underlying.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
      return underlying.GetEnumerator();
    }

    #endregion
  }

  public class AsObjectConverter : TypeConverter {
    public static SerializationContext DefaultSerializationContext = new SerializationContext();

    readonly SerializationContext serializationContext;

    public AsObjectConverter() {
      this.serializationContext = DefaultSerializationContext;
    }

    public AsObjectConverter(SerializationContext serializationContext) {
      this.serializationContext = serializationContext;
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
      if (destinationType.IsValueType || destinationType.IsEnum || destinationType.IsArray || destinationType.IsAbstract || destinationType.IsInterface)
        return false;
      return true;
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
      var instance = MethodFactory.CreateInstance(destinationType);
      var classDescription = serializationContext.GetClassDescription(destinationType, instance);
      var source = value as IDictionary<string, object>;
      if (source != null) {
        foreach (var kv in source) {
          IMemberWrapper wrapper;
          if (classDescription.TryGetMember(kv.Key, out wrapper))
            wrapper.SetValue(instance, kv.Value);
        }
        return instance;
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
