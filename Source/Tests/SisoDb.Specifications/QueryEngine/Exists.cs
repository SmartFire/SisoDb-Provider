using System;
using System.Collections.Generic;
using Machine.Specifications;
using SisoDb.Specifications.Model;
using SisoDb.Testing;

namespace SisoDb.Specifications.QueryEngine
{
    class Exists
    {
        [Subject(typeof(IReadSession), "Exists by Id (guid)")]
        public class when_set_with_guid_id_contains_match : SpecificationBase
        {
            Establish context = () =>
            {
                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
                TestContext.Database.WriteOnce().InsertMany(_structures);
            };

            Because of = () => 
                         _exists = TestContext.Database.ReadOnce().Exists<QueryGuidItem>(_structures[1].StructureId);

            It should_return_true = () => _exists.ShouldBeTrue();

            private static IList<QueryGuidItem> _structures;
            private static bool _exists;
        }

        [Subject(typeof(IReadSession), "Exists by Id (guid)")]
        public class when_set_with_guid_id_contains_no_match : SpecificationBase
        {
            Establish context = () =>
            {
                _fooId = Guid.Parse("f4bbe786-2231-4b62-b82c-22c5c3b4ed7d");

                TestContext = TestContextFactory.Create();
                _structures = QueryGuidItem.CreateFourItems<QueryGuidItem>();
                TestContext.Database.WriteOnce().InsertMany(_structures);
            };

            Because of = () =>
                         _exists = TestContext.Database.ReadOnce().Exists<QueryGuidItem>(_fooId);

            It should_return_false = () => _exists.ShouldBeFalse();

            private static Guid _fooId;
            private static IList<QueryGuidItem> _structures;
            private static bool _exists;
        }
    }
}