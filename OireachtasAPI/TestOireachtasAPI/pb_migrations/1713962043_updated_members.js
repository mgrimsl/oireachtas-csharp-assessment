migrate((db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("v0ybulvmd274rsh")

  // remove
  collection.schema.removeField("ghir5tcd")

  // add
  collection.schema.addField(new SchemaField({
    "system": false,
    "id": "m6wr2zcl",
    "name": "data",
    "type": "json",
    "required": false,
    "unique": false,
    "options": {}
  }))

  return dao.saveCollection(collection)
}, (db) => {
  const dao = new Dao(db)
  const collection = dao.findCollectionByNameOrId("v0ybulvmd274rsh")

  // add
  collection.schema.addField(new SchemaField({
    "system": false,
    "id": "ghir5tcd",
    "name": "member",
    "type": "json",
    "required": false,
    "unique": false,
    "options": {}
  }))

  // remove
  collection.schema.removeField("m6wr2zcl")

  return dao.saveCollection(collection)
})
