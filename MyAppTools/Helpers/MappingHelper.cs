using AutoMapper;

namespace MyAppTools.Helpers
{
    public interface IMappingHelper {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;
        TDest Map<TDest>(object source);
    }

    public class MappingHelper : IMappingHelper {
        public TDest Map<TSrc, TDest>(TSrc source) where TDest : class {
            return Mapper.Map<TSrc, TDest>(source);
        }

        public TDest Map<TDest>(object source) {
            return Mapper.Map<TDest>(source);
        }
    }
}
