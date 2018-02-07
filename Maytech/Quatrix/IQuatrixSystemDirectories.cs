namespace Maytech.Quatrix {


    public interface IQuatrixSystemDirectories {


        Metadata Home { get; }


        Metadata Inbox { get; }


        Metadata Outbox { get; }


        Metadata Trash { get; }


        Metadata SharedProjects { get; }
    }
}
