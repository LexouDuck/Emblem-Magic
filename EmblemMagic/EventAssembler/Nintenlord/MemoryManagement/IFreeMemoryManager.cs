using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nintenlord.MemoryManagement
{
    /// <summary>
    /// Manages memory
    /// </summary>
    public interface IFreeMemoryManager<TMemoryPointer> : IEnumerable<TMemoryPointer>, IAllocator<TMemoryPointer>
        where TMemoryPointer : IMemoryPointer
    {
        /// <summary>
        /// Add memory to manage
        /// </summary>
        /// <param name="offset">Offset of the memory to manage</param>
        /// <param name="size">Size of the memory to manage</param>
        /// <remarks>Same as AddManagedSpace(offset, size, false)</remarks>
        /// <exception cref="ArgumentException">Memory is already managed</exception>
        void AddManagedSpace(int offset, int size);
        /// <summary>
        /// Add memory to manage
        /// </summary>
        /// <param name="offset">Offset of the memory to manage</param>
        /// <param name="size">Size of the memory to manage</param>
        /// <param name="used">True if memory is already allocated, false if not</param>
        /// <returns>Managed memory pointer if memory is already used</returns>
        /// <exception cref="ArgumentException">Memory is already managed</exception>
        TMemoryPointer AddManagedSpace(int offset, int size, bool used);

        /// <summary>
        /// Stops memory from being managed
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentException">Memory isn't managed or it is allocated</exception>
        void RemoveManagedSpace(int offset, int size);
        /// <summary>
        /// Checks if memory is managed
        /// </summary>
        /// <param name="offset">Offset of memory to check</param>
        /// <param name="size">Size of the memory to check</param>
        /// <returns>True if pointer is valid and memory is managed, else false</returns>
        bool IsManaged(int offset, int size);
    }
}
