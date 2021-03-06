﻿using System;
using ProtoBuf;

namespace Byndyusoft.Net.Http.Formatting.ProtoBuf.Models
{
    [ProtoContract]
    public class People
    {
        [ProtoMember(1)] public int Id { get; set; }

        [ProtoMember(2)] public string Name { get; set; }

        [ProtoMember(3)] public DateTime DateOfBirth { get; set; }
    }
}