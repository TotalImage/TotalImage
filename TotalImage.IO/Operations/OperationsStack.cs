using System.Collections.Generic;
using System;
using System.Linq;

namespace TotalImage.Operations
{
    /// <summary>
    /// A stack of operations to be performed on the loaded image that can be either applied to the image or discarded.
    /// </summary>
    public class OperationsStack
    {
        private readonly List<Operation> _operations = [];

        /// <summary>
        /// Pushes a new operation to the top of the stack.
        /// </summary>
        /// <param name="operation">The operation to be pushed to the top of the stack.</param>
        public void Push(Operation operation)
        {
            _operations.Add(operation);
        }

        /// <summary>
        /// Pops and returns the operation from the top of the stack.
        /// </summary>
        /// <returns>The operation currently on top of the stack, or null when there are no operations currently on the stack.</returns>
        public Operation? Pop()
        {
            if (_operations.Count > 0)
            {
                Operation op = _operations[^1];
                _operations.RemoveAt(_operations.Count - 1);
                return op;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pops and returns the operation from the bottom of the stack.
        /// </summary>
        /// <returns>The operation currently at the bottom of the stack, or null when there are no operations currently on the stack.</returns>
        public Operation? PopFirst()
        {
            if (_operations.Count > 0)
            {
                Operation op = _operations[0];
                _operations.RemoveAt(0);
                return op;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pops and returns the operation at the provided index in the stack.
        /// </summary>
        /// <returns>The operation currently at the provided index in the stack, or null when there are no operations currently on the stack.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public Operation? PopAt(int index)
        {
            if(index < 0  || index >= _operations.Count)
                throw new IndexOutOfRangeException();

            if (_operations.Count > 0)
            {
                Operation op = _operations[index];
                _operations.RemoveAt(index);
                return op;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the operation currently on top of the stack without removing it.
        /// </summary>
        /// <returns>The operation currently on top of the stack, or null when there are no operations currently on the stack.</returns>
        public Operation? Peek()
        {
            return _operations.Count > 0 ? _operations[^1] : null;
        }

        /// <summary>
        /// Returns the operation currently at the bottom of the stack without removing it.
        /// </summary>
        /// <returns>The operation currently at the bottom of the stack, or null when there are no operations currently on the stack.</returns>
        public Operation? PeekFirst()
        {
            return _operations.Count > 0 ? _operations[0] : null;
        }

        /// <summary>
        /// Returns the operation currently at provided index in the stack without removing it.
        /// </summary>
        /// <returns>The operation currently at provided index in the stack, or null when there are no operations currently on the stack.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public Operation? PeekAt(int index)
        {
            if (index < 0 || index >= _operations.Count)
                throw new IndexOutOfRangeException();

            return _operations.Count > 0 ? _operations[index] : null;
        }

        /// <summary>
        /// Returns the number of operations currently on the stack.
        /// </summary>
        /// <returns>The number of operations currently on the stack.</returns>
        public int Count()
        {
            return _operations.Count;
        }

        /// <summary>
        /// Discards all operations currently on the stack.
        /// </summary>
        public void Discard()
        {
            _operations.Clear();
        }
    }
}
