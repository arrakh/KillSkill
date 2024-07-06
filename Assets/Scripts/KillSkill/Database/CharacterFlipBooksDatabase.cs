using System;
using System.Collections.Generic;
using System.Linq;
using Arr.ScriptableDatabases;
using FlipBooks;
using UnityEngine;

namespace KillSkill.Database
{
    
    [Serializable]
    public class CharacterFlipBooks : IScriptableKey<string>
    {
        [SerializeField] private string characterId;
        [SerializeField] private FlipBookAsset defaultFlipBook;
        [SerializeField] private FlipBookAsset[] flipBooks;

        public string Key => characterId;

        public FlipBook Default => defaultFlipBook.flipBook;
        public IEnumerable<FlipBook> FlipBooks => flipBooks.Select(x => x.flipBook);
    }
    
    [CreateAssetMenu(menuName = "Scriptable DBs/Character Flip Books DB")]
    public class CharacterFlipBooksDatabase : PairScriptableDatabase<string, CharacterFlipBooks>
    {
        
    }
}