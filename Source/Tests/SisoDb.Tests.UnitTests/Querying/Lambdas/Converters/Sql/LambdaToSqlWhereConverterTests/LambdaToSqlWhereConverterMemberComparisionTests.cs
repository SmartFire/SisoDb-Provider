﻿using NUnit.Framework;
using SisoDb.Querying.Lambdas.Converters.Sql;
using SisoDb.Tests.UnitTests.TestFactories;

namespace SisoDb.Tests.UnitTests.Querying.Lambdas.Converters.Sql.LambdaToSqlWhereConverterTests
{
    [TestFixture]
    public class LambdaToSqlWhereConverterMemberComparisionTests : LambdaToSqlWhereConverterTestBase
    {
        [Test]
        public void Process_WhenMemberOfSameType_GeneratesCorrectSqlQuery()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Int1 == i.Int2);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub(), parsedLambda);

            Assert.AreEqual("(si.[MemberPath]='Int1' and si.[IntegerValue] = (select sub.[IntegerValue] from [dbo].[TempIndexes] sub where sub.[MemberPath]='Int2'))", query.Sql);
        }

        [Test]
        public void Process_WhenMemberOfSameType_ExtractsCorrectParameters()
        {
            var parsedLambda = CreateParsedLambda<MyItem>(i => i.Int1 == i.Int2);

            var processor = new LambdaToSqlWhereConverter();
            var query = processor.Convert(StructureSchemaTestFactory.Stub(), parsedLambda);

            Assert.AreEqual(0, query.Parameters.Count);
        }
    }
}