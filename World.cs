using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace Desmos.Models
{
    public class World<T> where T : Entity
    {
        public IEnumerable<T> Entities { get; set; }
        public IEnumerable<Liaison> Liaisons { get; set; }
        public IEnumerable<LiaisonDefinition<T>> Properties { get; set; }

        public IEnumerable<Liaison> GetLiaisons(string entityId) =>
            Properties.SelectMany(p =>
                p.Rules(
                    entityId,
                    Liaisons,
                    Entities
                )
            ).DistinctBy(p => new { p.FromId, p.ToId, p.PropertyId });
    }

    /// <summary>
    /// Example where the world chosen is a world of people
    /// </summary>
    public class RelationshipWorld : World<Person>
    {
        public RelationshipWorld()
        {
            Properties = new List<LiaisonDefinition<Person>>
            {
                new LiaisonDefinition<Person> { Id = "parent", Rules = (entityId, liaisons, entities) => ParentDefinition(entityId, liaisons) },
                new LiaisonDefinition<Person> { Id = "mother", Rules = (entityId, liaisons, entities) => MotherDefinition(entityId, liaisons, entities) },
                new LiaisonDefinition<Person> { Id = "father", Rules = (entityId, liaisons, entities) => FatherDefinition(entityId, liaisons, entities) },
                new LiaisonDefinition<Person> { Id = "child", Rules = (entityId, liaisons, entities) => ChildDefinition(entityId, liaisons) },
                new LiaisonDefinition<Person> { Id = "daughter", Rules = (entityId, liaisons, entities) => DaughterDefinition(entityId, liaisons, entities) },
                new LiaisonDefinition<Person> { Id = "son", Rules = (entityId, liaisons, entities) => SonDefinition(entityId, liaisons, entities) },
                new LiaisonDefinition<Person> { Id = "sibling", Rules = (entityId, liaisons, entities) => SiblingDefinition(entityId, liaisons) },
                new LiaisonDefinition<Person> { Id = "sister", Rules = (entityId, liaisons, entities) => SisterDefinition(entityId, liaisons, entities) },
                new LiaisonDefinition<Person> { Id = "brother", Rules = (entityId, liaisons, entities) => BrotherDefinition(entityId, liaisons, entities) },
            };
        }

        private IEnumerable<Liaison> ParentDefinition(string entityId, IEnumerable<Liaison> liaisons) =>
            liaisons.Where(l => l.FromId == entityId && l.PropertyId == "parent");

        private IEnumerable<Liaison> MotherDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> parentLiaisons = ParentDefinition(entityId, liaisons);
            IEnumerable<Entity> mothers = entities.Where(e => parentLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Female);
            IEnumerable<Liaison> motherLiaisons = parentLiaisons.Where(l => mothers.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "mother" });

            return motherLiaisons;
        }

        private IEnumerable<Liaison> FatherDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> parentLiaisons = ParentDefinition(entityId, liaisons);
            IEnumerable<Entity> fathers = entities.Where(e => parentLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Male);
            IEnumerable<Liaison> fatherLiaisons = parentLiaisons.Where(l => fathers.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "father" });

            return fatherLiaisons;
        }

        private IEnumerable<Liaison> ChildDefinition(string entityId, IEnumerable<Liaison> liaisons) => liaisons
            .Where(l => l.ToId == entityId && l.PropertyId == "parent")
            .Select(l => new Liaison { FromId = l.ToId, ToId = l.FromId, PropertyId = "child" });

        private IEnumerable<Liaison> DaughterDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> childLiaisons = ChildDefinition(entityId, liaisons);
            IEnumerable<Entity> daughters = entities.Where(e => childLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Female);
            IEnumerable<Liaison> daughterLiaisons = childLiaisons.Where(l => daughters.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "daughter" });

            return daughterLiaisons;
        }

        private IEnumerable<Liaison> SonDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> childLiaisons = ChildDefinition(entityId, liaisons);
            IEnumerable<Entity> sons = entities.Where(e => childLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Male);
            IEnumerable<Liaison> sonLiaisons = childLiaisons.Where(l => sons.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "son" });

            return sonLiaisons;
        }

        private IEnumerable<Liaison> SiblingDefinition(string entityId, IEnumerable<Liaison> liaisons)
        {
            IEnumerable<Liaison> childLiaisons = ChildDefinition(entityId, liaisons);
            IEnumerable<Liaison> siblingLiaisons = childLiaisons
                .SelectMany(l => ParentDefinition(l.ToId, liaisons))
                .Where(l => l.ToId != entityId)
                .Select(l => new Liaison { FromId = entityId, ToId = l.ToId, PropertyId = "sibling" });

            return siblingLiaisons;
        }

        private IEnumerable<Liaison> SisterDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> siblingLiaisons = SiblingDefinition(entityId, liaisons);
            IEnumerable<Entity> sisters = entities.Where(e => siblingLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Female);
            IEnumerable<Liaison> sisterLiaisons = siblingLiaisons.Where(l => sisters.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "sister" });

            return sisterLiaisons;
        }

        private IEnumerable<Liaison> BrotherDefinition(string entityId, IEnumerable<Liaison> liaisons, IEnumerable<Person> entities)
        {
            IEnumerable<Liaison> siblingLiaisons = SiblingDefinition(entityId, liaisons);
            IEnumerable<Entity> brothers = entities.Where(e => siblingLiaisons.Select(l => l.FromId).Contains(e.Id) && e.GenderIdentity == GenderIdentity.Male);
            IEnumerable<Liaison> brotherLiaisons = siblingLiaisons.Where(l => brothers.Select(m => m.Id).Contains(l.FromId))
                .Select(l => new Liaison { FromId = l.FromId, ToId = l.ToId, PropertyId = "brother" });

            return brotherLiaisons;
        }
    }
}
