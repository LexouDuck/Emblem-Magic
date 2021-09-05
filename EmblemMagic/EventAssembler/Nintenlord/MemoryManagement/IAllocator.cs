using System;
namespace Nintenlord.MemoryManagement
{
    public interface IAllocator<TMemoryPointer>
     where TMemoryPointer : IMemoryPointer
    {
        /// <summary>
        /// Allocates memory
        /// </summary>
        /// <param name="size">Minimun size in bytes of allocated memory</param>
        /// <returns>Allocated memory or null pointer if out of memory</returns>
        /// <remarks>Same as Allocate(size, 1)</remarks>
        TMemoryPointer Allocate(int size);
        /// <summary>
        /// Allocates memory
        /// </summary>
        /// <param name="size">Minimun size in bytes of allocated memory</param>
        /// <param name="padding">The padding the returned offset</param>
        /// <returns>Allocated memory or null pointer if out of memory</returns>
        TMemoryPointer Allocate(int size, int padding);
        /// <summary>
        /// Deallocates memory
        /// </summary>
        /// <param name="pointer">Memory to deallocate</param>
        /// <exception cref="ArgumentException">Memory isn't allocated or pointer is null</exception>
        /// <exception cref="ArgumentNullException">Pointer is null</exception>
        void Deallocate(TMemoryPointer pointer);
        /// <summary>
        /// Checks if is allocated
        /// </summary>
        /// <param name="pointer">Memory to check</param>
        /// <returns>True if memory is allocated, else false</returns>
        /// <exception cref="ArgumentNullException">Pointer is null</exception>
        bool IsAllocated(TMemoryPointer pointer);
    }
}
