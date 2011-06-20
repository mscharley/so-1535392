using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Swizzle
{
        class Vector<T> : DynamicObject
        {
            private T[] m_values;
            private Dictionary<char, int> m_positions;

            public Vector(Dictionary<char, int> positions, params T[] values)
            {
                this.m_positions = positions;
                this.m_values = values;
            }

            public T this[int index] {
                get { return this.m_values[index]; }
            }

            public int Length
            {
                get { return this.m_values.Length; }
            }

            public override string ToString()
            {
                List<string> elements = new List<string>(this.Length);

                for (int i = 0; i < this.Length; i++)
                {
                    elements.Add(m_values[i].ToString());
                }

                return string.Join(", ", elements.ToArray());
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (binder.Name == "Length") {
                    result = this.Length;
                    return true;
                }

                if (binder.Name.Length == 1 && this.m_positions.ContainsKey(binder.Name[0]))
                {
                    result = m_values[this.m_positions[binder.Name[0]]];
                    return true;
                }

                Dictionary<char, int> positions = new Dictionary<char, int>(binder.Name.Length);
                List<T> values = new List<T>(binder.Name.Length);
                int i = 0;
                foreach (char c in binder.Name)
                {
                    if (!this.m_positions.ContainsKey(c))
                        return base.TryGetMember(binder, out result);

                    values.Add(m_values[m_positions[c]]);
                    positions.Add(c, i);

                    i++;
                }

                result = new Vector<T>(positions, values.ToArray());
                return true;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                // sanity checking.
                foreach (char c in binder.Name)
                {
                    if (!this.m_positions.ContainsKey(c))
                        return base.TrySetMember(binder, value);
                }

                Vector<T> vectorValue = value as Vector<T>;

                if (vectorValue == null && binder.Name.Length == 1 && value is T)
                {
                    m_values[m_positions[binder.Name[0]]] = (T)value;
                    return true;
                }
                else if (vectorValue == null)
                    throw new ArgumentException("You may only set properties of a Vector to another Vector of the same type.");
                if (vectorValue.Length != binder.Name.Length)
                    throw new ArgumentOutOfRangeException("The length of the Vector given does not match the length of the Vector to assign it to.");

                int i = 0;
                foreach (char c in binder.Name)
                {
                    m_values[m_positions[c]] = vectorValue[i];
                    i++;
                }

                return true;
            }
        }
}
