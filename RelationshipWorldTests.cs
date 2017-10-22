using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Desmos.Models.Tests.Unit
{
    public class RelationshipWorldTests
    {
        [Fact]
        public void test_no_liaisons()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "parent", GenderIdentity = GenderIdentity.Female },
                    new Person { Id = "father", GenderIdentity = GenderIdentity.Male },
                    new Person { Id = "child" },
                },
                Liaisons = new List<Liaison>
                {

                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("mother").ToList();

            // assert
            Assert.Empty(newLiaisons);
        }

        [Fact]
        public void test_mother_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "mother", GenderIdentity = GenderIdentity.Female },
                    new Person { Id = "child" },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "mother", ToId = "child", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("mother").ToList();

            // assert
            Assert.Equal("parent", newLiaisons[0].PropertyId);
            Assert.Equal("mother", newLiaisons[0].FromId);
            Assert.Equal("child", newLiaisons[0].ToId);

            Assert.Equal("mother", newLiaisons[1].PropertyId);
            Assert.Equal("mother", newLiaisons[1].FromId);
            Assert.Equal("child", newLiaisons[1].ToId);
        }


        [Fact]
        public void test_father_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "father", GenderIdentity = GenderIdentity.Male },
                    new Person { Id = "child" },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "father", ToId = "child", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("father").ToList();

            // assert
            Assert.Equal("parent", newLiaisons[0].PropertyId);
            Assert.Equal("father", newLiaisons[0].FromId);
            Assert.Equal("child", newLiaisons[0].ToId);

            Assert.Equal("father", newLiaisons[1].PropertyId);
            Assert.Equal("father", newLiaisons[1].FromId);
            Assert.Equal("child", newLiaisons[1].ToId);
        }

        [Fact]
        public void test_daughter_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "parent" },
                    new Person { Id = "daughter", GenderIdentity = GenderIdentity.Female },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "parent", ToId = "daughter", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("daughter").ToList();

            // assert
            Assert.Equal("child", newLiaisons[0].PropertyId);
            Assert.Equal("daughter", newLiaisons[0].FromId);
            Assert.Equal("parent", newLiaisons[0].ToId);

            Assert.Equal("daughter", newLiaisons[1].PropertyId);
            Assert.Equal("daughter", newLiaisons[1].FromId);
            Assert.Equal("parent", newLiaisons[1].ToId);
        }

        [Fact]
        public void test_son_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "parent" },
                    new Person { Id = "son", GenderIdentity = GenderIdentity.Male },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "parent", ToId = "son", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("son").ToList();

            // assert
            Assert.Equal("child", newLiaisons[0].PropertyId);
            Assert.Equal("son", newLiaisons[0].FromId);
            Assert.Equal("parent", newLiaisons[0].ToId);

            Assert.Equal("son", newLiaisons[1].PropertyId);
            Assert.Equal("son", newLiaisons[1].FromId);
            Assert.Equal("parent", newLiaisons[1].ToId);
        }

        [Fact]
        public void test_sister_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "parent" },
                    new Person { Id = "daughter", GenderIdentity = GenderIdentity.Female },
                    new Person { Id = "sibling" },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "parent", ToId = "daughter", PropertyId = "parent" },
                    new Liaison { FromId = "parent", ToId = "sibling", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("daughter").ToList();

            // assert
            Assert.Equal("child", newLiaisons[0].PropertyId);
            Assert.Equal("daughter", newLiaisons[0].FromId);
            Assert.Equal("parent", newLiaisons[0].ToId);

            Assert.Equal("daughter", newLiaisons[1].PropertyId);
            Assert.Equal("daughter", newLiaisons[1].FromId);
            Assert.Equal("parent", newLiaisons[1].ToId);

            Assert.Equal("sibling", newLiaisons[2].PropertyId);
            Assert.Equal("daughter", newLiaisons[2].FromId);
            Assert.Equal("sibling", newLiaisons[2].ToId);

            Assert.Equal("sister", newLiaisons[3].PropertyId);
            Assert.Equal("daughter", newLiaisons[3].FromId);
            Assert.Equal("sibling", newLiaisons[3].ToId);
        }

        [Fact]
        public void test_brother_definition()
        {
            // arrange
            var world1 = new RelationshipWorld
            {
                Entities = new List<Person>
                {
                    new Person { Id = "parent" },
                    new Person { Id = "son", GenderIdentity = GenderIdentity.Male },
                    new Person { Id = "sibling" },
                },
                Liaisons = new List<Liaison>
                {
                    new Liaison { FromId = "parent", ToId = "son", PropertyId = "parent" },
                    new Liaison { FromId = "parent", ToId = "sibling", PropertyId = "parent" },
                }
            };

            // act
            List<Liaison> newLiaisons = world1.GetLiaisons("son").ToList();

            // assert
            Assert.Equal("child", newLiaisons[0].PropertyId);
            Assert.Equal("son", newLiaisons[0].FromId);
            Assert.Equal("parent", newLiaisons[0].ToId);

            Assert.Equal("son", newLiaisons[1].PropertyId);
            Assert.Equal("son", newLiaisons[1].FromId);
            Assert.Equal("parent", newLiaisons[1].ToId);

            Assert.Equal("sibling", newLiaisons[2].PropertyId);
            Assert.Equal("son", newLiaisons[2].FromId);
            Assert.Equal("sibling", newLiaisons[2].ToId);

            Assert.Equal("brother", newLiaisons[3].PropertyId);
            Assert.Equal("son", newLiaisons[3].FromId);
            Assert.Equal("sibling", newLiaisons[3].ToId);
        }
    }
}
