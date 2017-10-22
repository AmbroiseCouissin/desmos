using System;
using System.Collections.Generic;

namespace Desmos.Models
{
    public class Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Person : Entity
    {
        public GenderIdentity GenderIdentity { get; set; }
    }

    public class LiaisonDefinition<T> where T : Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Func<string, IEnumerable<Liaison>, IEnumerable<T>, IEnumerable<Liaison>> Rules { get; set; }
    }

    public enum GenderIdentity
    {
        Female,
        Male,
        Agender,
        Bigender,
    }
}
