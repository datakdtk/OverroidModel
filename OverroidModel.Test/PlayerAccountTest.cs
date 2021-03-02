using System.Collections.Generic;
using Xunit;

namespace OverroidModel.Test
{
    public class PlayerAccountTest
    {
        [Fact]
        public void Test_Equality_Equal()
        {
            var a = new PlayerAccount("hoge");
            var b = new PlayerAccount("hoge");
            Assert.Equal(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
        }

        [Fact]
        public void Test_Equality_EqualWhenDifferentDisplayName()
        {
            var a = new PlayerAccount("hoge", "name1");
            var b = new PlayerAccount("hoge", "name2");
            Assert.Equal(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
        }

        [Fact]
        public void Test_Equality_NotEqual()
        {
            var a = new PlayerAccount("hoge");
            var b = new PlayerAccount("fuga");
            Assert.NotEqual(a, b);
            Assert.False(a == b);
            Assert.True(a != b);
        }

        [Fact]
        public void Test_Equality_WithNull()
        {
            var a = new PlayerAccount("hoge");
            PlayerAccount? b = null;
            Assert.NotEqual(a, b);
            Assert.False(a == b);
            Assert.True(a != b);
        }

        [Fact]
        public void Test_Equality_WithNull_2()
        {
            var a = new PlayerAccount("hoge");
            Assert.False(a == null);
            Assert.True(a != null);
        }

        [Fact]
        public void Test_Equality_WithNull_3()
        {
            PlayerAccount? a = null;
            Assert.True(a == null);
            Assert.False(a != null);
        }


        [Fact]
        public void Test_BehaviourAsHashKey()
        {
            var a = new PlayerAccount("hoge");
            var b = new PlayerAccount("hoge");
            var c = new PlayerAccount("fuga");
            var dic = new Dictionary<PlayerAccount, int>();

            dic[a] = 0;
            dic[b] = 1;
            Assert.Equal(1, dic[a]);
            Assert.Single(dic.Keys);

            dic[c] = 2;
            Assert.Equal(1, dic[a]);
            Assert.Equal(2, dic[c]);
            Assert.Equal(2, dic.Keys.Count);
        }

        [Fact]
        public void Test_BehaviourAsHashKey_WhenDifferentDisplayName()
        {
            var a = new PlayerAccount("hoge", "name1");
            var b = new PlayerAccount("hoge", "name2");
            var dic = new Dictionary<PlayerAccount, int>();

            dic[a] = 0;
            dic[b] = 1;
            Assert.Equal(1, dic[a]);
            Assert.Single(dic.Keys);
        }
    }
}
