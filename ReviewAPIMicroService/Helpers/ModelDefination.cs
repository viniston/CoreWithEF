using System;
using System.Collections;
using System.Linq;

namespace ReviewAPIMicroService.Helpers
{
    /// <summary>
    /// Model definition.
    /// </summary>
    internal class ModelDefination
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDefination"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ModelDefination(object model)
        {
            if (model == null)
            {
                return;
            }

            ModelType = model.GetType().IsGenericType
                ? model.GetType().GenericTypeArguments.FirstOrDefault()
                : model.GetType();
            ModelValue = model;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is enumerable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enumerable; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnumerable => ModelValue is IEnumerable;

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; private set; }

        /// <summary>
        /// Gets the model value.
        /// </summary>
        /// <value>
        /// The model value.
        /// </value>
        public object ModelValue { get; }

        /// <summary>
        /// Executes the on model.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        public void ExecuteOnModel(Action<object> operation)
        {
            if (!IsEnumerable)
            {
                operation?.Invoke(ModelValue);
            }
            else
            {
                foreach (var value in (IEnumerable)ModelValue)
                {
                    operation?.Invoke(value);
                }
            }
        }
    }
}
