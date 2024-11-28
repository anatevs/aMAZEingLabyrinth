using System;
using System.Linq;
using System.Reflection;
using VContainer;

namespace VContainerExt
{
    public static class ObjectResolverExtension
    {
        public static T ResolveInstance<T>(this IObjectResolver objectResolver)
        {
            Type type = typeof(T);
            ConstructorInfo constructorInfo = type.GetConstructors().FirstOrDefault();

            if (constructorInfo is null)
            {
                throw new VContainerException(type, "Failed to find suitable constructor!");
            }

            ParameterInfo[] parameters = constructorInfo.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = objectResolver.Resolve(parameters[i].ParameterType);
            }

            T instance = (T)constructorInfo.Invoke(args);
            objectResolver.Inject(instance);

            return instance;
        }
    }
}