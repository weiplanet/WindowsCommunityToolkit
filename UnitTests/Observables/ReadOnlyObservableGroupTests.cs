﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentAssertions;
using Microsoft.Toolkit.Observables.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace UnitTests.Observables
{
    [TestClass]
    public class ReadOnlyObservableGroupTests
    {
        [TestCategory("Observables")]
        [TestMethod]
        public void Ctor_WithKeyAndCollection_ShouldHaveExpectedInitialState()
        {
            var source = new ObservableCollection<int>(new[] { 1, 2, 3 });
            var group = new ReadOnlyObservableGroup<string, int>("key", source);

            group.Key.Should().Be("key");
            group.Should().BeEquivalentTo(new[] { 1, 2, 3 }, option => option.WithStrictOrdering());
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Ctor_ObservableGroup_ShouldHaveExpectedInitialState()
        {
            var source = new[] { 1, 2, 3 };
            var sourceGroup = new ObservableGroup<string, int>("key", source);
            var group = new ReadOnlyObservableGroup<string, int>(sourceGroup);

            group.Key.Should().Be("key");
            group.Should().BeEquivalentTo(new[] { 1, 2, 3 }, option => option.WithStrictOrdering());
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Add_ShouldRaiseEvent()
        {
            var collectionChangedEventRaised = false;
            var source = new[] { 1, 2, 3 };
            var sourceGroup = new ObservableGroup<string, int>("key", source);
            var group = new ReadOnlyObservableGroup<string, int>(sourceGroup);
            ((INotifyCollectionChanged)group).CollectionChanged += (s, e) => collectionChangedEventRaised = true;

            sourceGroup.Add(4);

            group.Key.Should().Be("key");
            group.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 }, option => option.WithStrictOrdering());
            collectionChangedEventRaised.Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Update_ShouldRaiseEvent()
        {
            var collectionChangedEventRaised = false;
            var source = new[] { 1, 2, 3 };
            var sourceGroup = new ObservableGroup<string, int>("key", source);
            var group = new ReadOnlyObservableGroup<string, int>(sourceGroup);
            ((INotifyCollectionChanged)group).CollectionChanged += (s, e) => collectionChangedEventRaised = true;

            sourceGroup[1] = 4;

            group.Key.Should().Be("key");
            group.Should().BeEquivalentTo(new[] { 1, 4, 3 }, option => option.WithStrictOrdering());
            collectionChangedEventRaised.Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Remove_ShouldRaiseEvent()
        {
            var collectionChangedEventRaised = false;
            var source = new[] { 1, 2, 3 };
            var sourceGroup = new ObservableGroup<string, int>("key", source);
            var group = new ReadOnlyObservableGroup<string, int>(sourceGroup);
            ((INotifyCollectionChanged)group).CollectionChanged += (s, e) => collectionChangedEventRaised = true;

            sourceGroup.Remove(1);

            group.Key.Should().Be("key");
            group.Should().BeEquivalentTo(new[] { 2, 3 }, option => option.WithStrictOrdering());
            collectionChangedEventRaised.Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Clear_ShouldRaiseEvent()
        {
            var collectionChangedEventRaised = false;
            var source = new[] { 1, 2, 3 };
            var sourceGroup = new ObservableGroup<string, int>("key", source);
            var group = new ReadOnlyObservableGroup<string, int>(sourceGroup);
            ((INotifyCollectionChanged)group).CollectionChanged += (s, e) => collectionChangedEventRaised = true;

            sourceGroup.Clear();

            group.Key.Should().Be("key");
            group.Should().BeEmpty();
            collectionChangedEventRaised.Should().BeTrue();
        }
    }
}
