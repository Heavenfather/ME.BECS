namespace ME.BECS {
    
    using static Cuts;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using System.Diagnostics;
    using Unity.Collections.LowLevel.Unsafe;
    using Internal;

    public static class JournalConditionals {

        public const string JOURNAL = "JOURNAL";

    }

    public enum JournalAction : long {

        Unknown = 0,
        
        CreateComponent  = 1 << 0,
        UpdateComponent  = 1 << 1,
        RemoveComponent  = 1 << 2,
        EnableComponent  = 1 << 3,
        DisableComponent = 1 << 4,
        
        SystemAdded         = 1 << 5,
        SystemUpdateStarted = 1 << 6,
        SystemUpdateEnded   = 1 << 7,
        
        EntityUpVersion = 1 << 8,
        
        All = CreateComponent | UpdateComponent | RemoveComponent | EnableComponent | DisableComponent | SystemAdded | SystemUpdateStarted | SystemUpdateEnded | EntityUpVersion,
        
    }

    [System.Serializable]
    public struct JournalProperties {

        public static JournalProperties Default => new JournalProperties() {
            capacity = 1000u,
            historyCapacity = 10000u,
        };

        [UnityEngine.Tooltip("Journal items capacity per thread.")]
        public uint capacity;

        [UnityEngine.Tooltip("Journal items history capacity per thread.")]
        public uint historyCapacity;

    }

    public unsafe struct JournalsStorage {

        public struct Item {

            public Journal* journal;

        }

        private static readonly Unity.Burst.SharedStatic<Array<Item>> journalsArrBurst = Unity.Burst.SharedStatic<Array<Item>>.GetOrCreatePartiallyUnsafeWithHashCode<JournalsStorage>(TAlign<Array<Item>>.align, 10101);
        internal static ref Array<Item> journals => ref journalsArrBurst.Data;

        public static void Set(uint id, Journal* journal) {
            if (id >= journals.Length) {
                journals.Resize((id + 1u) * 2u);
            }
            journals.Get(id) = new Item() {
                journal = journal,
            };
        }

        public static Journal* Get(uint id) {
            if (id >= journals.Length) return null;
            return journals.Get(id).journal;
        }
        
    }

    public unsafe partial struct Journal {
        
        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void EnableComponent<T>(ushort worldId, in Ent ent) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->EnableComponent<T>(in ent);

        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void DisableComponent<T>(ushort worldId, in Ent ent) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->DisableComponent<T>(in ent);

        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void SetComponent<T>(ushort worldId, in Ent ent, in T data) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            if (ent.Has<T>() == true) {
                journal->UpdateComponent<T>(in ent, in data);
            } else {
                journal->CreateComponent<T>(in ent, in data);
            }

        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void CreateComponent<T>(ushort worldId, in Ent ent, in T data) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->CreateComponent<T>(in ent, in data);
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void UpdateComponent<T>(ushort worldId, in Ent ent, in T data) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->UpdateComponent<T>(in ent, in data);
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void RemoveComponent<T>(ushort worldId, in Ent ent) where T : unmanaged, IComponent {

            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->RemoveComponent<T>(in ent);

        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void AddSystem(ushort worldId, Unity.Collections.FixedString64Bytes name) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->AddSystem(name);
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void UpdateSystemStarted(ushort worldId, Unity.Collections.FixedString64Bytes name) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->UpdateSystemStarted(name);
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void UpdateSystemEnded(ushort worldId, Unity.Collections.FixedString64Bytes name) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->UpdateSystemEnded(name);
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void BeginFrame(ushort worldId) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->BeginFrame();
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void EndFrame(ushort worldId) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->EndFrame();
            
        }

        [INLINE(256)]
        [Conditional(JournalConditionals.JOURNAL)]
        public static void VersionUp(ushort worldId, in Ent ent) {
            
            var journal = JournalsStorage.Get(worldId);
            if (journal == null) return;
            journal->VersionUp(in ent);

        }

    }

    public unsafe partial struct Journal : System.IDisposable {

        private World* world;
        private JournalData* data;
        private bool isCreated;

        public JournalData* GetData() => this.data;
        public World* GetWorld() => this.world;

        [INLINE(256)]
        public static Journal Create(in World connectedWorld, in JournalProperties properties) {

            var props = WorldProperties.Default;
            props.name = $"Journal for #{connectedWorld.id}";
            var world = World.Create(props);
            var journal = new Journal {
                world = _make(world),
                data = _make(JournalData.Create(world.state, properties)),
                isCreated = true,
            };
            return journal;

        }

        public struct EntityJournal {

            public struct Item {

                public ulong tick;
                public Unity.Collections.NativeList<JournalItem> events;

            }
            
            public Unity.Collections.NativeHashMap<ulong, Item> eventsPerTick;

            public void Add(in JournalItem data) {

                if (this.eventsPerTick.TryGetValue(data.tick, out var item) == true) {

                    item.tick = data.tick;
                    item.events.Add(in data);
                    this.eventsPerTick[data.tick] = item;

                } else {

                    item = new Item() {
                        tick = data.tick,
                        events = new Unity.Collections.NativeList<JournalItem>(Constants.ALLOCATOR_TEMP),
                    };
                    item.events.Add(in data);
                    this.eventsPerTick.Add(data.tick, item);

                }

            }

        }
        
        public EntityJournal GetEntityJournal(in Ent ent) {

            var entityJournal = new EntityJournal();
            var items = this.data->GetData();
            entityJournal.eventsPerTick = new Unity.Collections.NativeHashMap<ulong, EntityJournal.Item>(10, Constants.ALLOCATOR_TEMP);
            ulong startTick = 0UL;
            for (uint i = 0; i < items.Length; ++i) {
                var item = items[this.world->state, i];
                var tick = item.historyStartTick;
                if (tick > startTick) {
                    startTick = tick;
                }
            }

            for (uint i = 0; i < items.Length; ++i) {
                var item = items[this.world->state, i];
                var e = item.historyItems.GetEnumerator(this.world->state);
                while (e.MoveNext() == true) {
                    var journalItem = e.Current;
                    if (journalItem.tick >= startTick && journalItem.ent == ent) {
                        entityJournal.Add(journalItem);
                    }
                }
                e.Dispose();
            }
            return entityJournal;

        }

        [INLINE(256)]
        public void Dispose() {

            if (this.world == null) return;
            if (this.data != null) this.data->Dispose(this.world->state);
            this.world->Dispose();
            this = default;

        }
        
        [INLINE(256)]
        public void AddSystem(Unity.Collections.FixedString64Bytes name) {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { action = JournalAction.SystemAdded, name = name, });

        }

        [INLINE(256)]
        public void UpdateSystemStarted(Unity.Collections.FixedString64Bytes name) {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { action = JournalAction.SystemUpdateStarted, name = name, });

        }
        
        [INLINE(256)]
        public void UpdateSystemEnded(Unity.Collections.FixedString64Bytes name) {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { action = JournalAction.SystemUpdateEnded, name = name, });

        }

        [INLINE(256)]
        public void CreateComponent<T>(in Ent ent, in T data) where T : unmanaged, IComponent {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.CreateComponent, typeId = StaticTypes<T>.typeId, storeInHistory = true, });

        }

        [INLINE(256)]
        public void UpdateComponent<T>(in Ent ent, in T data) where T : unmanaged, IComponent {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.UpdateComponent, typeId = StaticTypes<T>.typeId, storeInHistory = true, });

        }

        [INLINE(256)]
        public void RemoveComponent<T>(in Ent ent) where T : unmanaged, IComponent {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.RemoveComponent, typeId = StaticTypes<T>.typeId, storeInHistory = true, });

        }

        [INLINE(256)]
        public void EnableComponent<T>(in Ent ent) where T : unmanaged, IComponent {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.EnableComponent, typeId = StaticTypes<T>.typeId, storeInHistory = true, });

        }

        [INLINE(256)]
        public void DisableComponent<T>(in Ent ent) where T : unmanaged, IComponent {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.DisableComponent, typeId = StaticTypes<T>.typeId, storeInHistory = true, });

        }

        [INLINE(256)]
        public void VersionUp(in Ent ent) {

            if (this.isCreated == false) return;
            this.data->Add(this.world->state, new JournalItem() { ent = ent, action = JournalAction.EntityUpVersion, data = ent.Version, storeInHistory = true, });

        }

        [INLINE(256)]
        public void BeginFrame() {

            if (this.isCreated == false) return;
            this.data->Clear(this.world->state);

        }

        [INLINE(256)]
        public void EndFrame() {

        }

    }

    public unsafe struct JournalItem {

        [INLINE(256)]
        public static JournalItem Create(JournalItem source) {
            source.threadIndex = Unity.Jobs.LowLevel.Unsafe.JobsUtility.ThreadIndex;
            if (source.ent.IsAlive() == true) {
                source.tick = source.ent.World.state->tick;
            } else {
                source.tick = Context.world.state->tick;
            }
            return source;
        }

        public bool storeInHistory;
        public ulong tick;
        public Unity.Collections.FixedString64Bytes name;
        public long data;
        public Ent ent;
        public JournalAction action;
        public uint typeId;
        public int threadIndex;

        public void Dispose() {
            //if (this.data != null) _free(ref this.data);
            this = default;
        }

        public override string ToString() {
            return $"Tick: {this.tick}, ent: {this.ent}, action: {this.action}, typeId: {this.typeId}";
        }

        public string GetClass() {
            return this.action.ToString();
        }

    }

    public unsafe struct JournalData {

        public struct ThreadItem {

            public Queue<JournalItem> items;
            public Queue<JournalItem> historyItems;
            public ulong historyStartTick;
            private readonly JournalProperties properties;

            public ThreadItem(State* state, in JournalProperties properties) {
                this.items = new Queue<JournalItem>(ref state->allocator, properties.capacity);
                this.historyItems = new Queue<JournalItem>(ref state->allocator, properties.historyCapacity);
                this.historyStartTick = 0UL;
                this.properties = properties;
            }

            [INLINE(256)]
            public void Add(State* state, JournalItem journalItem) {
            
                journalItem = JournalItem.Create(journalItem);
                this.TryAddToHistory(state, journalItem);
                if (this.items.Count >= this.properties.capacity) {
                    var item = this.items.Dequeue(ref state->allocator);
                    item.Dispose();
                }
                this.items.Enqueue(ref state->allocator, journalItem);

            }

            [INLINE(256)]
            private void TryAddToHistory(State* state, JournalItem item) {
                if (item.storeInHistory == true) {
                    if (this.historyItems.Count >= this.properties.historyCapacity) {
                        var historyItem = this.historyItems.Dequeue(ref state->allocator);
                        this.historyStartTick = historyItem.tick + 1UL;
                        historyItem.Dispose();
                    }
                    this.historyItems.Enqueue(ref state->allocator, item);
                }
            }

            [INLINE(256)]
            public void Clear(State* state) {
            
                var e = this.items.GetEnumerator(state);
                while (e.MoveNext() == true) {
                    e.Current.Dispose();
                }
                e.Dispose();
                this.items.Clear();

            }

            [INLINE(256)]
            public void Dispose(State* state) {

                {
                    var e = this.items.GetEnumerator(state);
                    while (e.MoveNext() == true) {
                        e.Current.Dispose();
                    }
                    e.Dispose();
                }
                {
                    var e = this.historyItems.GetEnumerator(state);
                    while (e.MoveNext() == true) {
                        e.Current.Dispose();
                    }
                    e.Dispose();
                }
                this = default;

            }

        }

        private MemArray<ThreadItem> threads;

        public MemArray<ThreadItem> GetData() => this.threads;

        [INLINE(256)]
        public static JournalData Create(State* state, in JournalProperties properties) {
            
            var journal = new JournalData {
                threads = new MemArray<ThreadItem>(ref state->allocator, (uint)Unity.Jobs.LowLevel.Unsafe.JobsUtility.ThreadIndexCount),
            };
            for (uint i = 0u; i < journal.threads.Length; ++i) {
                journal.threads[state, i] = new ThreadItem(state, properties);
            }
            return journal;

        }

        [INLINE(256)]
        public void Add(State* state, JournalItem journalItem) {
            
            this.threads[state, Unity.Jobs.LowLevel.Unsafe.JobsUtility.ThreadIndex].Add(state, journalItem);

        }

        [INLINE(256)]
        public void Clear(State* state) {

            for (uint i = 0u; i < this.threads.Length; ++i) {
                this.threads[state, i].Clear(state);
            }

        }

        [INLINE(256)]
        public void Dispose(State* state) {
            
            for (uint i = 0u; i < this.threads.Length; ++i) {
                this.threads[state, i].Dispose(state);
            }
            
        }

    }

}