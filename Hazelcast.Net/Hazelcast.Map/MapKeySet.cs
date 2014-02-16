using System;
using System.Collections.Generic;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using Hazelcast.Serialization.Hook;

namespace Hazelcast.Map
{
    [Serializable]
    public class MapKeySet : IdentifiedDataSerializable,IIdentifiedDataSerializable
    {
        internal ICollection<Data> keySet;

        public MapKeySet(ICollection<Data> keySet)
        {
            this.keySet = keySet;
        }

        public MapKeySet()
        {
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual void WriteData(IObjectDataOutput output)
        {
            int size = keySet.Count;
            output.WriteInt(size);
            foreach (Data o in keySet)
            {
                o.WriteData(output);
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual void ReadData(IObjectDataInput input)
        {
            int size = input.ReadInt();
            keySet = new HashSet<Data>();
            for (int i = 0; i < size; i++)
            {
                var data = new Data();
                data.ReadData(input);
                keySet.Add(data);
            }
        }

        public virtual int GetFactoryId()
        {
            return MapDataSerializerHook.FId;
        }

        public virtual int GetId()
        {
            return MapDataSerializerHook.KeySet;
        }

        public virtual ICollection<Data> GetKeySet()
        {
            return keySet;
        }
    }
}