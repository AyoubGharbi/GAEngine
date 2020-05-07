using GAEngine.Components;
using GAEngine.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAEngine.Managers
{
    class ComponentManager<T> where T : IComponent
    {
        private Stack<T> _components= new Stack<T>();
        private Stack<Entity> _entities = new Stack<Entity>();

        public int Count => _components.Count;

        public Tuple<T, Entity> this[int i] => Tuple.Create(_components.ElementAt(i), _entities.ElementAt(i));

        public T CreateEntity(Entity entity, T component)
        {
            _entities.Push(entity);
             _components.Push(component);

            return _components.Peek();
        }

        public void CleanUp()
        {
            foreach (var component in _components)
            {
                component.CleanUp();
            }
        }
    }
}
