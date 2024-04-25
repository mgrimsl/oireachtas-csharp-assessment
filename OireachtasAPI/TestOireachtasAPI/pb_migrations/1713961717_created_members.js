migrate((db) => {
  const collection = new Collection({
    "id": "v0ybulvmd274rsh",
    "created": "2024-04-24 12:28:37.322Z",
    "updated": "2024-04-24 12:28:37.322Z",
    "name": "members",
    "type": "base",
    "system": false,
    "schema": [
      {
        "system": false,
        "id": "ghir5tcd",
        "name": "member",
        "type": "json",
        "required": false,
        "unique": false,
        "options": {}
      }
    ],
    "indexes": [],
    "listRule": null,
    "viewRule": null,
    "createRule": null,
    "updateRule": null,
    "deleteRule": null,
    "options": {}
  });

  return Dao(db).saveCollection(collection);
}, (db) => {
  const dao = new Dao(db);
  const collection = dao.findCollectionByNameOrId("v0ybulvmd274rsh");

  return dao.deleteCollection(collection);
})
