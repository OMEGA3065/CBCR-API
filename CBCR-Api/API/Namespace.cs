namespace CustomRoleLib.API
{
    /// <summary>
    /// An object namespace.
    /// </summary>
    public class ObjectNamespace
    {
        private ObjectNamespace() {}

        /// <summary>
        /// The plugin part of the namespace (PLUGIN_PART:ID_PART)
        /// </summary>
        public string PluginNamespace;

        /// <summary>
        /// The ID part of the namespace (PLUGIN_PART:ID_PART)
        /// </summary>
        public string RoleIdentifier;

        /// <summary>
        /// Tries to obtain an <see cref="ObjectNamespace"/> from a string in the namespace format.
        /// <example>
        /// For example:
        /// <code>RoleNamespace.TryGet("my_plugin:my_custom_item", out var @namespace)</code>
        /// will return true and the parsed <see cref="ObjectNamespace"/> will be in <c>@namespace</c>.
        /// </example>
        /// </summary>
        /// <param name="namespaceString">The <see cref="string"/> to try and parse.</param>
        /// <param name="namespace">The parsed <see cref="ObjectNamespace"/>.</param>
        /// <returns>Whether the namespace has been parsed successfully.</returns>
        public static bool TryGet(string namespaceString, out ObjectNamespace @namespace)
        {
            var split = namespaceString.Split(':');
            if (split.Length != 2)
            {
                @namespace = null;
                return false;
            }

            @namespace = new ObjectNamespace
            {
                PluginNamespace = split[0],
                RoleIdentifier = split[1]
            };
            return true;
        }

        /// <summary>
        /// Parses a <c>namespaceString</c> to an <see cref="ObjectNamespace"/>.
        /// </summary>
        /// <param name="namespaceString">The <see cref="string"/> to parse.</param>
        /// <returns>The parsed <see cref="ObjectNamespace"/>.</returns>
        /// <exception cref="ArgumentException">For invalid <c>namespaceString</c>s</exception>
        public static ObjectNamespace Get(string namespaceString)
        {
            if (TryGet(namespaceString, out var @namespace))
                return @namespace;

            throw new ArgumentException("Invalid namespace format. Expected format: 'plugin_namespace:item_identifier'");
        }

        /// <summary>
        /// Parses a <c>pluginNamespace</c> with an <c>itemIdentifier</c> to an <see cref="ObjectNamespace"/>.
        /// </summary>
        /// <param name="pluginNamespace">The Plugin Namespace part of an <see cref="ObjectNamespace"/> to parse.</param>
        /// <param name="itemIdentifier">The Item ID part of an <see cref="ObjectNamespace"/> to parse.</param>
        /// <returns>The parsed <see cref="ObjectNamespace"/>.</returns>
        /// <exception cref="ArgumentException">For invalid <c>namespaceString</c>s</exception>
        public static ObjectNamespace Get(string pluginNamespace, string itemIdentifier)
        {
            return new ObjectNamespace
            {
                PluginNamespace = pluginNamespace,
                RoleIdentifier = itemIdentifier
            };
        }

        /// <summary>
        /// Parses this <see cref="ObjectNamespace"/> to a <see cref="string"/>.
        /// </summary>
        /// <returns>The parsed <see cref="string"/>.</returns>
        public override string ToString()
        {
            return $"{PluginNamespace}:{RoleIdentifier}";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is not ObjectNamespace) return false;
            return obj.ToString() == ToString();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(ObjectNamespace a, ObjectNamespace b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null ^ b is null) return false;
            return a.Equals(b);
        }

        /// <inheritdoc/>
        public static bool operator !=(ObjectNamespace a, ObjectNamespace b)
        {
            if (ReferenceEquals(a, b)) return false;
            if (a is null ^ b is null) return true;
            return !a.Equals(b);
        }
    }
}