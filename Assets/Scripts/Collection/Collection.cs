using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class Collection<T>
{
    /// <summary>
    /// Nom de la collection.
    /// </summary>
    [Header("Collection Settings")]
    [SerializeField]
    private string collectionName;

    /// <summary>
    /// Liste des éléments de la collection.
    /// </summary>
    [Header("Collection Elements")]
    [SerializeField]
    private List<T> list = new List<T>();

    /// <summary>
    /// Dictionnaire associant des clés uniques à des éléments de la collection.
    /// </summary>
    [SerializeField]
    private SerializableDictionary<string, T> dictionary = new SerializableDictionary<string, T>();

    #region Events

    /// <summary>
    /// Délégué d'événement appelé lorsqu'un nouvel élément est ajouté à la collection.
    /// </summary>
    /// <param name="item">L'élément ajouté à la collection.</param>
    public delegate void ItemAddedEventHandler(T item);

    /// <summary>
    /// Délégué d'événement appelé lorsqu'un élément est retiré de la collection.
    /// </summary>
    /// <param name="item">L'élément retiré de la collection.</param>
    public delegate void ItemRemovedEventHandler(T item);

    /// <summary>
    /// Délégué d'événement appelé lorsqu'une collection est entièrement vidée.
    /// </summary>
    public delegate void CollectionClearedEventHandler();

    /// <summary>
    /// Événement déclenché lorsqu'un nouvel élément est ajouté à la collection.
    /// </summary>
    public event ItemAddedEventHandler ItemAdded;

    /// <summary>
    /// Événement déclenché lorsqu'un élément est retiré de la collection.
    /// </summary>
    public event ItemRemovedEventHandler ItemRemoved;

    /// <summary>
    /// Événement déclenché lorsqu'une collection est entièrement vidée.
    /// </summary>
    public event CollectionClearedEventHandler CollectionCleared;

    #endregion

    #region Index

    /// <summary>
    /// Vérifie si l'index spécifié est en dehors de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index à vérifier.</param>
    /// <returns>true si l'index est en dehors de la plage valide, sinon false.</returns>
    public bool IsOutOfIndex(int index)
    {
        return index < 0 || index >= list.Count;
    }

    /// <summary>
    /// Vérifie si l'index spécifié est à l'intérieur de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index à vérifier.</param>
    /// <returns>true si l'index est à l'intérieur de la plage valide, sinon false.</returns>
    public bool IsInOfIndex(int index)
    {
        return index >= 0 && index < list.Count;
    }

    #endregion

    #region Has

    /// <summary>
    /// Vérifie si la collection contient un élément avec la clé spécifiée.
    /// </summary>
    /// <param name="key">La clé de l'élément à rechercher dans la collection.</param>
    /// <returns>true si la collection contient un élément avec la clé spécifiée, sinon false.</returns>
    public bool HasItem(string key)
    {
        if (dictionary.ContainsKey(key)) return true;
        return false;
    }

    /// <summary>
    /// Vérifie si la collection contient l'élément spécifié.
    /// </summary>
    /// <param name="item">L'élément à rechercher dans la collection.</param>
    /// <returns>true si la collection contient l'élément spécifié, sinon false.</returns>
    public bool HasItem(T item)
    {
        if (list.Contains(item)) return true;
        return false;
    }

    #endregion

    #region Add

    /// <summary>
    /// Ajoute un élément à la collection.
    /// </summary>
    /// <param name="item">L'élément entré.</param>
    /// /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddToList(T item)
    {
        string key = item.ToString();
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    public void AddItemToDictionary(T item)
    {
        string key = item.ToString();
        if (!HasItem(key))
        {
            dictionary.Add(key, item);
        }
    }

    /// <summary>
    /// Ajoute un élément défaut à la collection.
    /// </summary>
    /// <param name="key">La clé associée à l'élément par défaut.</param>
    /// /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddItem(string key)
    {
        if (!HasItem(key))
        {
            T item = default;
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute un nouvel élément à la collection avec la clé spécifiée.
    /// </summary>
    /// <param name="key">La clé de l'élément à ajouter.</param>
    /// <param name="item">L'élément à ajouter à la collection.</param>
    /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddItem(string key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item); // Déclenche l'événement ItemAdded pour notifier les abonnés.
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute une série d'éléments à la collection avec des clés générées à partir de la clé spécifiée.
    /// </summary>
    /// <param name="key">La clé de base pour générer les clés des éléments ajoutés.</param>
    /// <param name="items">Les éléments à ajouter à la collection.</param>
    /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection, ce qui empêche de créer une succession de clés à partir de ce nom.</exception>
    public void AddItems(string key, IEnumerable<T> items)
    {
        if (!HasItem(key))
        {
            int index = 0;
            foreach (T item in items)
            {
                string uniquekey = key + index;
                if (!HasItem(uniquekey))
                {
                    list.Add(item);
                    dictionary.Add(uniquekey, item);
                    ItemAdded?.Invoke(item);
                }
                index++;
            }
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection, impossible de créer une succession de clé à partir de ce nom.");
        }
    }

    #endregion

    #region Getter

    /// <summary>
    /// Récupère le nom de la collection.
    /// </summary>
    /// <returns>Le nom de la collection.</returns>
    public string GetCollectionName() { return collectionName; }

    /// <summary>
    /// Récupère la clé associée à l'élément spécifié dans la collection.
    /// </summary>
    /// <param name="item">L'élément dont la clé doit être récupérée.</param>
    /// <returns>La clé associée à l'élément, ou null si aucun élément correspondant n'est trouvé.</returns>
    public string GetKeyByItem(T item)
    {
        foreach (var kvp in dictionary)
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item)) return kvp.Key;
        return null;
    }

    /// <summary>
    /// Récupère l'élément associé à la clé spécifiée dans la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à récupérer.</param>
    /// <returns>
    /// L'élément associé à la clé spécifiée s'il est trouvé dans la collection,
    /// sinon la valeur par défaut de type T.
    /// </returns>
    public T GetItemBykey(string key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Récupère l'élément à l'index spécifié dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'élément à récupérer.</param>
    /// <returns>
    /// L'élément à l'index spécifié dans la collection, ou la valeur par défaut de type T si l'index est invalide.
    /// </returns>
    public T GetItemByIndex(int index)
    {
        if (IsOutOfIndex(index)) return default(T);
        return list[index];
    }

    /// <summary>
    /// Récupère une liste contenant tous les éléments de la collection.
    /// </summary>
    /// <returns>Une liste contenant tous les éléments de la collection.</returns>
    public List<T> GetItemsList() { return list; }

    /// <summary>
    /// Récupère un dictionnaire contenant toutes les clés et leurs éléments correspondants de la collection.
    /// </summary>
    /// <returns>Un dictionnaire contenant toutes les clés et leurs éléments correspondants de la collection.</returns>
    public SerializableDictionary<string, T> GetItemsDictionary() { return dictionary; }

    /// <summary>
    /// Récupère le nombre d'éléments dans la collection.
    /// </summary>
    /// <returns>Le nombre d'éléments dans la collection.</returns>
    public int GetItemsNumber() { return list.Count; }

    #endregion

    #region Find

    /// <summary>
    /// Recherche et récupère l'élément associé à la clé spécifiée dans la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à rechercher.</param>
    /// <returns>
    /// L'élément associé à la clé spécifiée s'il est trouvé dans la collection,
    /// sinon la valeur par défaut de type T.
    /// </returns>
    public T FindItemBykey(string key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Recherche et récupère l'élément à l'index spécifié dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'élément à rechercher.</param>
    /// <returns>
    /// L'élément à l'index spécifié dans la collection,
    /// ou la valeur par défaut de type T si l'index est invalide.
    /// </returns>
    public T FindItemByIndex(int index)
    {
        if (IsInOfIndex(index)) return list[index];
        return default(T);
    }

    /// <summary>
    /// Recherche et récupère la clé associée à l'élément spécifié dans la collection.
    /// </summary>
    /// <param name="item">L'élément dont la clé doit être recherchée.</param>
    /// <returns>
    /// La clé associée à l'élément spécifié, ou null si aucun élément correspondant n'est trouvé.
    /// </returns>
    public string FindKeyByItem(T item)
    {
        string keyToFind = null;
        foreach (var kvp in dictionary)
        {
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
            {
                keyToFind = kvp.Key;
                break;
            }
        }
        return keyToFind;
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui correspondent au prédicat spécifié.
    /// </summary>
    /// <param name="predicate">Le prédicat utilisé pour déterminer les éléments à récupérer.</param>
    /// <returns>Une liste contenant tous les éléments de la collection qui correspondent au prédicat spécifié.</returns>
    public List<T> FindItems(Predicate<T> predicate)
    {
        return list.FindAll(predicate);
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui satisfont la condition spécifiée.
    /// </summary>
    /// <param name="condition">La fonction qui spécifie la condition à vérifier pour chaque élément.</param>
    /// <returns>Une liste contenant tous les éléments de la collection qui satisfont la condition spécifiée.</returns>
    public List<T> FindItems(Func<T, bool> condition)
    {
        return list.Where(condition).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui ont une propriété spécifique égale à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="value">La valeur à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifique est égale à la valeur donnée.</returns>
    public List<T> FindItemsByProperty(Func<T, object> propertySelector, object value)
    {
        return list.Where(item => propertySelector(item).Equals(value)).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée contient une correspondance partielle avec un motif donné.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="pattern">Le motif à rechercher dans la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée contient une correspondance partielle avec le motif donné.</returns>
    public List<T> FindItemsByPartialMatch(Func<T, string> propertySelector, string pattern)
    {
        return list.Where(item => propertySelector(item).Contains(pattern)).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée a une valeur minimale égale ou supérieure à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="minValue">La valeur minimale à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée a une valeur minimale égale ou supérieure à la valeur donnée.</returns>
    public List<T> FindItemsByMinValue(Func<T, int> propertySelector, int minValue)
    {
        return list.Where(item => propertySelector(item) >= minValue).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée a une valeur maximale égale ou inférieure à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="maxValue">La valeur maximale à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée a une valeur maximale égale ou inférieure à la valeur donnée.</returns>
    public List<T> FindItemsByMaxValue(Func<T, int> propertySelector, int maxValue)
    {
        return list.Where(item => propertySelector(item) <= maxValue).ToList();
    }

    #endregion

    #region Setter

    /// <summary>
    /// Définit le nom de la collection.
    /// </summary>
    /// <param name="value">Le nouveau nom de la collection.</param>
    public void SetCollectionName(string value) { collectionName = value; }

    #endregion

    #region Remove

    /// <summary>
    /// Supprime l'élément associé à la clé spécifiée de la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à supprimer.</param>
    /// <exception cref="ArgumentException">Levée si aucune clé correspondante n'est trouvée dans la collection.</exception>
    public void RemoveItemBykey(string key)
    {
        if (HasItem(key))
        {
            T item = FindItemBykey(key);
            list.Remove(item);
            dictionary.Remove(key);
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime l'élément spécifié de la collection.
    /// </summary>
    /// <param name="item">L'élément à supprimer de la collection.</param>
    /// <exception cref="ArgumentException">Levée si l'élément spécifié n'est pas présent dans la collection.</exception>
    public void RemoveItemBySelf(T item)
    {
        if (HasItem(item))
        {
            list.Remove(item);
            dictionary.Remove(FindKeyByItem(item));
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Cet item n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime tous les éléments spécifiés de la collection.
    /// </summary>
    /// <param name="items">Une collection d'éléments à supprimer de la collection.</param>
    /// <remarks>
    /// Cette méthode parcourt la liste des éléments spécifiés et supprime chaque élément de la collection.
    /// </remarks>
    public void RemoveItemsBySelf(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            RemoveItemBySelf(item);
        }
    }

    /// <summary>
    /// Supprime tous les éléments associés aux clés spécifiées de la collection.
    /// </summary>
    /// <param name="keys">Une collection de clés à supprimer de la collection.</param>
    /// <remarks>
    /// Cette méthode parcourt la liste des clés spécifiées et supprime chaque élément associé à chaque clé de la collection.
    /// </remarks>
    public void RemoveItemsByKey(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            RemoveItemBykey(key);
        }
    }

    #endregion

    #region Clear

    /// <summary>
    /// Supprime tous les éléments de la collection.
    /// </summary>
    /// <remarks>
    /// Cette méthode vide à la fois la liste et le dictionnaire de la collection.
    /// Elle déclenche également l'événement <see cref="CollectionCleared"/> après avoir vidé la collection.
    /// </remarks>
    public void ClearItems()
    {
        list.Clear();
        dictionary.Clear();
        CollectionCleared?.Invoke();
    }

    #endregion

    #region Save

    /// <summary>
    /// Sérialise la liste d'éléments de la collection et les sauvegarde dans un fichier binaire spécifié.
    /// </summary>
    /// <param name="filePath">Le chemin d'accès complet du fichier de sauvegarde.</param>
    public void Save(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, list);
        }
    }

    #endregion

    #region Load

    /// <summary>
    /// Charge une liste d'éléments à partir d'un fichier binaire spécifié et les assigne à la collection.
    /// </summary>
    /// <param name="filePath">Le chemin d'accès complet du fichier de sauvegarde.</param>
    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                list = (List<T>)formatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("Le fichier de sauvegarde n'existe pas.");
        }
    }

    #endregion

    public void OnDestroy()
    {
        ClearItems();
    }
}


[System.Serializable]
public class Collection<U, T>
{
    /// <summary>
    /// Nom de la collection.
    /// </summary>
    [Header("Collection Settings")]
    [SerializeField]
    private string collectionName;

    /// <summary>
    /// Liste des éléments de la collection.
    /// </summary>
    [Header("Collection Elements")]
    [SerializeField]
    private List<T> list = new List<T>();

    /// <summary>
    /// Dictionnaire associant des clés uniques à des éléments de la collection.
    /// </summary>
    [SerializeField]
    private SerializableDictionary<U, T> dictionary = new SerializableDictionary<U, T>();

    #region Events

    /// <summary>
    /// Délégué d'événement appelé lorsqu'un nouvel élément est ajouté à la collection.
    /// </summary>
    /// <param name="item">L'élément ajouté à la collection.</param>
    public delegate void ItemAddedEventHandler(T item);

    /// <summary>
    /// Délégué d'événement appelé lorsqu'un élément est retiré de la collection.
    /// </summary>
    /// <param name="item">L'élément retiré de la collection.</param>
    public delegate void ItemRemovedEventHandler(T item);

    /// <summary>
    /// Délégué d'événement appelé lorsqu'une collection est entièrement vidée.
    /// </summary>
    public delegate void CollectionClearedEventHandler();

    /// <summary>
    /// Événement déclenché lorsqu'un nouvel élément est ajouté à la collection.
    /// </summary>
    public event ItemAddedEventHandler ItemAdded;

    /// <summary>
    /// Événement déclenché lorsqu'un élément est retiré de la collection.
    /// </summary>
    public event ItemRemovedEventHandler ItemRemoved;

    /// <summary>
    /// Événement déclenché lorsqu'une collection est entièrement vidée.
    /// </summary>
    public event CollectionClearedEventHandler CollectionCleared;

    #endregion

    #region Index

    /// <summary>
    /// Vérifie si l'index spécifié est en dehors de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index à vérifier.</param>
    /// <returns>true si l'index est en dehors de la plage valide, sinon false.</returns>
    public bool IsOutOfIndex(int index)
    {
        return index < 0 || index >= list.Count;
    }

    /// <summary>
    /// Vérifie si l'index spécifié est à l'intérieur de la plage valide des indices de la collection.
    /// </summary>
    /// <param name="index">L'index à vérifier.</param>
    /// <returns>true si l'index est à l'intérieur de la plage valide, sinon false.</returns>
    public bool IsInOfIndex(int index)
    {
        return index >= 0 && index < list.Count;
    }

    #endregion

    #region Has

    /// <summary>
    /// Vérifie si la collection contient un élément avec la clé spécifiée.
    /// </summary>
    /// <param name="key">La clé de l'élément à rechercher dans la collection.</param>
    /// <returns>true si la collection contient un élément avec la clé spécifiée, sinon false.</returns>
    public bool HasItem(U key)
    {
        if (dictionary.ContainsKey(key)) return true;
        return false;
    }

    /// <summary>
    /// Vérifie si la collection contient l'élément spécifié.
    /// </summary>
    /// <param name="item">L'élément à rechercher dans la collection.</param>
    /// <returns>true si la collection contient l'élément spécifié, sinon false.</returns>
    public bool HasItem(T item)
    {
        if (list.Contains(item)) return true;
        return false;
    }

    #endregion

    #region Add

    /// <summary>
    /// Ajoute un élément à la collection.
    /// </summary>
    /// <param name="item">L'élément entré.</param>
    /// /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddToList(U key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    public void AddItemToDictionary(U key, T item)
    {
        if (!HasItem(key))
        {
            dictionary.Add(key, item);
        }
    }

    /// <summary>
    /// Ajoute un élément défaut à la collection.
    /// </summary>
    /// <param name="key">La clé associée à l'élément par défaut.</param>
    /// /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddItem(U key)
    {
        if (!HasItem(key))
        {
            T item = default;
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute un nouvel élément à la collection avec la clé spécifiée.
    /// </summary>
    /// <param name="key">La clé de l'élément à ajouter.</param>
    /// <param name="item">L'élément à ajouter à la collection.</param>
    /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection.</exception>
    public void AddItem(U key, T item)
    {
        if (!HasItem(key))
        {
            list.Add(item);
            dictionary.Add(key, item);
            ItemAdded?.Invoke(item); // Déclenche l'événement ItemAdded pour notifier les abonnés.
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection.");
        }
    }

    /// <summary>
    /// Ajoute une série d'éléments à la collection avec des clés générées à partir de la clé spécifiée.
    /// </summary>
    /// <param name="keys">Les clés des éléments ajoutés.</param>
    /// <param name="items">Les éléments à ajouter à la collection.</param>
    /// <exception cref="ArgumentException">Levée si une clé avec le même nom existe déjà dans la collection, ce qui empêche de créer une succession de clés à partir de ce nom.</exception>
    public void AddItems(IEnumerable<U> keys, IEnumerable<T> items)
    {
        int index = 0;
        foreach (T item in items)
        {
            if (!HasItem(keys.ElementAtOrDefault(index)))
            {
                list.Add(item);
                dictionary.Add(keys.ElementAtOrDefault(index), item);
                ItemAdded?.Invoke(item);
            }
            else
            {
                throw new ArgumentException("Une clé avec ce nom existe déjà dans la collection, impossible de créer une succession de clé à partir de ce nom.");
            }
            index++;
        }
    }

    #endregion

    #region Getter

    /// <summary>
    /// Récupère le nom de la collection.
    /// </summary>
    /// <returns>Le nom de la collection.</returns>
    public string GetCollectionName() { return collectionName; }

    /// <summary>
    /// Récupère la clé associée à l'élément spécifié dans la collection.
    /// </summary>
    /// <param name="item">L'élément dont la clé doit être récupérée.</param>
    /// <returns>La clé associée à l'élément, ou null si aucun élément correspondant n'est trouvé.</returns>
    public U GetKeyByItem(T item)
    {
        foreach (var kvp in dictionary)
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item)) return kvp.Key;
        return default(U);
    }

    /// <summary>
    /// Récupère l'élément associé à la clé spécifiée dans la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à récupérer.</param>
    /// <returns>
    /// L'élément associé à la clé spécifiée s'il est trouvé dans la collection,
    /// sinon la valeur par défaut de type T.
    /// </returns>
    public T GetItemBykey(U key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Récupère l'élément à l'index spécifié dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'élément à récupérer.</param>
    /// <returns>
    /// L'élément à l'index spécifié dans la collection, ou la valeur par défaut de type T si l'index est invalide.
    /// </returns>
    public T GetItemByIndex(int index)
    {
        if (IsOutOfIndex(index)) return default(T);
        return list[index];
    }

    /// <summary>
    /// Récupère une liste contenant tous les éléments de la collection.
    /// </summary>
    /// <returns>Une liste contenant tous les éléments de la collection.</returns>
    public List<T> GetItemsList() { return list; }

    /// <summary>
    /// Récupère un dictionnaire contenant toutes les clés et leurs éléments correspondants de la collection.
    /// </summary>
    /// <returns>Un dictionnaire contenant toutes les clés et leurs éléments correspondants de la collection.</returns>
    public SerializableDictionary<U, T> GetItemsDictionary() { return dictionary; }

    /// <summary>
    /// Récupère le nombre d'éléments dans la collection.
    /// </summary>
    /// <returns>Le nombre d'éléments dans la collection.</returns>
    public int GetItemsNumber() { return list.Count; }

    #endregion

    #region Find

    /// <summary>
    /// Recherche et récupère l'élément associé à la clé spécifiée dans la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à rechercher.</param>
    /// <returns>
    /// L'élément associé à la clé spécifiée s'il est trouvé dans la collection,
    /// sinon la valeur par défaut de type T.
    /// </returns>
    public T FindItemBykey(U key)
    {
        if (HasItem(key)) return dictionary[key];
        return default(T);
    }

    /// <summary>
    /// Recherche et récupère l'élément à l'index spécifié dans la collection.
    /// </summary>
    /// <param name="index">L'index de l'élément à rechercher.</param>
    /// <returns>
    /// L'élément à l'index spécifié dans la collection,
    /// ou la valeur par défaut de type T si l'index est invalide.
    /// </returns>
    public T FindItemByIndex(int index)
    {
        if (IsInOfIndex(index)) return list[index];
        return default(T);
    }

    /// <summary>
    /// Recherche et récupère la clé associée à l'élément spécifié dans la collection.
    /// </summary>
    /// <param name="item">L'élément dont la clé doit être recherchée.</param>
    /// <returns>
    /// La clé associée à l'élément spécifié, ou null si aucun élément correspondant n'est trouvé.
    /// </returns>
    public U FindKeyByItem(T item)
    {
        U keyToFind = default(U);
        foreach (var kvp in dictionary)
        {
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
            {
                keyToFind = kvp.Key;
                break;
            }
        }
        return keyToFind;
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui correspondent au prédicat spécifié.
    /// </summary>
    /// <param name="predicate">Le prédicat utilisé pour déterminer les éléments à récupérer.</param>
    /// <returns>Une liste contenant tous les éléments de la collection qui correspondent au prédicat spécifié.</returns>
    public List<T> FindItems(Predicate<T> predicate)
    {
        return list.FindAll(predicate);
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui satisfont la condition spécifiée.
    /// </summary>
    /// <param name="condition">La fonction qui spécifie la condition à vérifier pour chaque élément.</param>
    /// <returns>Une liste contenant tous les éléments de la collection qui satisfont la condition spécifiée.</returns>
    public List<T> FindItems(Func<T, bool> condition)
    {
        return list.Where(condition).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection qui ont une propriété spécifique égale à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="value">La valeur à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifique est égale à la valeur donnée.</returns>
    public List<T> FindItemsByProperty(Func<T, object> propertySelector, object value)
    {
        return list.Where(item => propertySelector(item).Equals(value)).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée contient une correspondance partielle avec un motif donné.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="pattern">Le motif à rechercher dans la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée contient une correspondance partielle avec le motif donné.</returns>
    public List<T> FindItemsByPartialMatch(Func<T, string> propertySelector, string pattern)
    {
        return list.Where(item => propertySelector(item).Contains(pattern)).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée a une valeur minimale égale ou supérieure à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="minValue">La valeur minimale à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée a une valeur minimale égale ou supérieure à la valeur donnée.</returns>
    public List<T> FindItemsByMinValue(Func<T, int> propertySelector, int minValue)
    {
        return list.Where(item => propertySelector(item) >= minValue).ToList();
    }

    /// <summary>
    /// Recherche et récupère tous les éléments de la collection dont la propriété spécifiée a une valeur maximale égale ou inférieure à une valeur donnée.
    /// </summary>
    /// <param name="propertySelector">La fonction de sélection de propriété utilisée pour extraire la propriété spécifique de chaque élément.</param>
    /// <param name="maxValue">La valeur maximale à comparer avec la propriété spécifique des éléments.</param>
    /// <returns>Une liste contenant tous les éléments de la collection dont la propriété spécifiée a une valeur maximale égale ou inférieure à la valeur donnée.</returns>
    public List<T> FindItemsByMaxValue(Func<T, int> propertySelector, int maxValue)
    {
        return list.Where(item => propertySelector(item) <= maxValue).ToList();
    }

    #endregion

    #region Setter

    /// <summary>
    /// Définit le nom de la collection.
    /// </summary>
    /// <param name="value">Le nouveau nom de la collection.</param>
    public void SetCollectionName(string value) { collectionName = value; }

    #endregion

    #region Remove

    /// <summary>
    /// Supprime l'élément associé à la clé spécifiée de la collection.
    /// </summary>
    /// <param name="key">La clé de l'élément à supprimer.</param>
    /// <exception cref="ArgumentException">Levée si aucune clé correspondante n'est trouvée dans la collection.</exception>
    public void RemoveItemBykey(U key)
    {
        if (HasItem(key))
        {
            T item = FindItemBykey(key);
            list.Remove(item);
            dictionary.Remove(key);
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Une clé avec ce nom n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime l'élément spécifié de la collection.
    /// </summary>
    /// <param name="item">L'élément à supprimer de la collection.</param>
    /// <exception cref="ArgumentException">Levée si l'élément spécifié n'est pas présent dans la collection.</exception>
    public void RemoveItemBySelf(T item)
    {
        if (HasItem(item))
        {
            list.Remove(item);
            dictionary.Remove(FindKeyByItem(item));
            ItemRemoved?.Invoke(item);
        }
        else
        {
            throw new ArgumentException("Cet item n'existe pas dans la collection.");
        }
    }

    /// <summary>
    /// Supprime tous les éléments spécifiés de la collection.
    /// </summary>
    /// <param name="items">Une collection d'éléments à supprimer de la collection.</param>
    /// <remarks>
    /// Cette méthode parcourt la liste des éléments spécifiés et supprime chaque élément de la collection.
    /// </remarks>
    public void RemoveItemsBySelf(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            RemoveItemBySelf(item);
        }
    }

    /// <summary>
    /// Supprime tous les éléments associés aux clés spécifiées de la collection.
    /// </summary>
    /// <param name="keys">Une collection de clés à supprimer de la collection.</param>
    /// <remarks>
    /// Cette méthode parcourt la liste des clés spécifiées et supprime chaque élément associé à chaque clé de la collection.
    /// </remarks>
    public void RemoveItemsByKey(IEnumerable<U> keys)
    {
        foreach (var key in keys)
        {
            RemoveItemBykey(key);
        }
    }

    #endregion

    #region Clear

    /// <summary>
    /// Supprime tous les éléments de la collection.
    /// </summary>
    /// <remarks>
    /// Cette méthode vide à la fois la liste et le dictionnaire de la collection.
    /// Elle déclenche également l'événement <see cref="CollectionCleared"/> après avoir vidé la collection.
    /// </remarks>
    public void ClearItems()
    {
        list.Clear();
        dictionary.Clear();
        CollectionCleared?.Invoke();
    }

    #endregion

    #region Save

    /// <summary>
    /// Sérialise la liste d'éléments de la collection et les sauvegarde dans un fichier binaire spécifié.
    /// </summary>
    /// <param name="filePath">Le chemin d'accès complet du fichier de sauvegarde.</param>
    public void Save(string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, list);
        }
    }

    #endregion

    #region Load

    /// <summary>
    /// Charge une liste d'éléments à partir d'un fichier binaire spécifié et les assigne à la collection.
    /// </summary>
    /// <param name="filePath">Le chemin d'accès complet du fichier de sauvegarde.</param>
    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                list = (List<T>)formatter.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("Le fichier de sauvegarde n'existe pas.");
        }
    }

    #endregion

    public void OnDestroy()
    {
        ClearItems();
    }
}