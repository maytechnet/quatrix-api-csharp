namespace Maytech.Quatrix.Entity {


    public interface IQMetadata : IQEntity {


        string name { get; }


        string parent_id { get; }


        long modified { get; }


        long size { get; }


        MetadataType Type { get; }
    }
}
