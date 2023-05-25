# School Test App

## TODO 6 ANSWER :
First, the getFromCache(id) function gets the data from AccountCache.
So we need to change data exactly in cache.
Basically, this is what it says in the comment above.

Second, the Get function retrieves data from the database and clones this object.
That's why even if we tried to change the database data using the "account.Counter++" line,
we wouldn't succeed. Since this field is already being changed from a cloned object.

So the solution is this. Use the Get function to get the current account.
Then use its InternalId to search for this account in the cache.
And in it already change the Counter field.And it will work, because we get object from cache by reference.
