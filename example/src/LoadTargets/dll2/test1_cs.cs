using t4_interface;

namespace ddl2
{
    public partial class test1_cs : ITransformText
    {
        private ClassInfo m_data;
        public test1_cs(ClassInfo data) { this.m_data = data; }
    }

    public class ClassInfo
    {
        public string ClassName { set; get; }
    }
}
