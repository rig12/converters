namespace GPNA.Converters.Tests.Services
{
    #region Using
    using NUnit.Framework;
    using Microsoft.Extensions.Logging;
    using Moq;
    using GPNA.Converters.TagValues;
    #endregion Using

    public class TagValueConverterUnitTests
    {
        #region Fields
        private TagValueConverterTestable _tagValueConverterTestable;
        #endregion Fields


        #region Constants
        private const string TEST_STRING = "test";
        private const int TEST_INT32 = 123;
        private const long TEST_INT64 = 99999999999;
        #endregion Constants


        #region Initialization
        [SetUp]
        public void Initialization()
        {
            var moqLogger = new Mock<ILogger<TagValueConverterTestable>>();
            _tagValueConverterTestable = new TagValueConverterTestable(moqLogger.Object);
        }
        #endregion Initialization


        #region Methods
        [Test]
        public void GetTagValue_by_empty_string_ResultMustBe_null()
        {
            Assert.IsTrue(_tagValueConverterTestable.GetTagValue(string.Empty) == default);
        }

        [Test]
        public void GetTagValue_by_test_string_string_ResultMustBe_Null()
        {
            Assert.IsTrue(_tagValueConverterTestable.GetTagValue(TEST_STRING) == default);
        }

        [Test]
        public void GetTagValue_by_TagValueDouble_json_string_ResultMustBe_TagValueDouble()
        {
            var entity = new TagValueDouble()
            {
                Value = 123.2
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueDouble && (result as TagValueDouble).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDouble_with_empty_value_json_string_ResultMustBe_TagValueNull()
        {
            var entity = new TagValueDouble();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueNull);
        }

        [Test]
        public void GetTagValue_by_TagValueBool_json_string_ResultMustBe_TagValueBool()
        {
            var entity = new TagValueBool()
            {
                Value = true
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueBool && (result as TagValueBool).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueBool_with_default_value_json_string_ResultMustBe_TagValueBool()
        {
            var entity = new TagValueBool();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueBool && (result as TagValueBool).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDateTime_json_string_ResultMustBe_TagValueDateTime()
        {
            var entity = new TagValueDateTime()
            {
                Value = System.DateTime.UtcNow
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueDateTime && (result as TagValueDateTime).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDateTime_with_default_value_json_string_ResultMustBe_TagValueDateTime()
        {
            var entity = new TagValueDateTime();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueDateTime && (result as TagValueDateTime).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueString_json_string_ResultMustBe_TagValueString()
        {
            var entity = new TagValueString()
            {
                Value = TEST_STRING
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueString && (result as TagValueString).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueString_with_empty_value_json_string_ResultMustBe_TagValueNull()
        {
            var entity = new TagValueString();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueNull);
        }

        [Test]
        public void GetTagValue_by_TagValueInt64_json_string_ResultMustBe_TagValueInt64()
        {
            var entity = new TagValueInt64()
            {
                Value = TEST_INT64
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueInt64 && (result as TagValueInt64).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueInt64_with_int32_value_json_string_ResultMustBe_TagValueInt64()
        {
            var entity = new TagValueInt64()
            {
                Value = TEST_INT32
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueInt64 && (result as TagValueInt64).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueInt64_with_empty_value_json_string_ResultMustBe_TagValueNull()
        {
            var entity = new TagValueInt64();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueNull);
        }

        [Test]
        public void GetTagValue_by_TagValueInt32_json_string_ResultMustBe_TagValueInt64()
        {
            var entity = new TagValueInt32()
            {
                Value = TEST_INT32
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueInt64 && (result as TagValueInt64).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueInt32_with_empty_value_json_string_ResultMustBe_TagValueNull()
        {
            var entity = new TagValueInt32();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueNull);
        }

        [Test]
        public void GetTagValue_by_TagValueDynamic_with_int32_value_json_string_ResultMustBe_TagValueInt64()
        {
            var entity = new TagValueDynamic()
            {
                Value = TEST_INT32
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueInt64 && (result as TagValueInt64).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDynamic_with_int64_value_json_string_ResultMustBe_TagValueInt64()
        {
            var entity = new TagValueDynamic()
            {
                Value = TEST_INT64
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueInt64 && (result as TagValueInt64).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDynamic_with_string_value_json_string_ResultMustBe_TagValueString()
        {
            var entity = new TagValueDynamic()
            {
                Value = TEST_STRING
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueString && (result as TagValueString).Value == entity.Value);
        }

        [Test]
        public void GetTagValue_by_TagValueDynamic_with_empty_value_json_string_ResultMustBe_TagValueNull()
        {
            var entity = new TagValueDynamic();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            var result = _tagValueConverterTestable.GetTagValue(json);
            Assert.IsTrue(result is TagValueNull);
        }
        #endregion Methods
    }
}
