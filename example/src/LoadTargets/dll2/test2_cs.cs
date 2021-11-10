using t4_interface;

namespace ddl2
{
    public partial class test2_cs : ITransformText
    {
        private ClassInfo2 m_data;
        public test2_cs(ClassInfo2 data) { this.m_data = data; }
    }

    public class ClassInfo2
    {
        public string ClassName { set; get; }
    }
}
