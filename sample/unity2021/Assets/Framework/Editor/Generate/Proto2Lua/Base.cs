using System.Collections.Generic;

namespace MakeLuaProtoConfig
{
    abstract class Base
    {
        protected List<ProtoData> protoList = new List<ProtoData>();

        protected abstract void ParseProto(string item);
    }
}