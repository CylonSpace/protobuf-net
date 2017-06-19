﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ProtoBuf;
using System.IO;

namespace Examples.Issues
{
    
    public class Issue192
    {
        [ProtoContract]
        class SomeType { }
        [ProtoContract]
        class Wrapper
        {
            [ProtoMember(1)]
            public List<SomeType>[] List { get; set; }
        }
        // the important thing is that this error is identical to the one from SerializeWrappedDeepList
        [Fact]
        public void SerializeDeepList()
        {
            Program.ExpectFailure<NotSupportedException>(() =>
            {
                var list = new List<SomeType>[] { new List<SomeType> { new SomeType() }, new List<SomeType> { new SomeType() } };
                Serializer.Serialize(Stream.Null, list);
            }, "Nested or jagged lists and arrays are not supported");
        }
        [Fact]
        public void DeserializeDeepList()
        {
            Program.ExpectFailure<NotSupportedException>(() =>
            {
                Serializer.Deserialize<List<SomeType>[]>(Stream.Null);
            }, "Nested or jagged lists and arrays are not supported");
        }
        [Fact]
        public void SerializeWrappedDeepList()
        {
            Program.ExpectFailure<NotSupportedException>(() =>
            {
                var wrapped = new Wrapper();
                var clone = Serializer.DeepClone(wrapped);
            }, "Nested or jagged lists and arrays are not supported");
        }

    }
}
