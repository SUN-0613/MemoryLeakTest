namespace MemoryLeakTest.Data
{

    /// <summary>一覧に表示する内容</summary>
    public class CollectionData
    {

        /// <summary>SEQ</summary>
        public int SEQ { get; private set; }

        /// <summary>乱数</summary>
        public string Value { get; private set; }

        /// <summary>一覧に表示する内容</summary>
        /// <param name="index">一覧のindex</param>
        public CollectionData(int index, int value)
        {

            SEQ = index + 1;
            Value = value.ToString("N0");

        }

    }

}
