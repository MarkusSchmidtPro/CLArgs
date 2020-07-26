namespace MSPro.CLArgs
{
    public class VerbDescriptor
    {
       public VerbDescriptor(string tag, string description)
        {
            this.Tag         = tag;
            this.Description = description;
        }


        public string Tag { get; set; }
        public string Description { get; set; }
    }
}