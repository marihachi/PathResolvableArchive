# PathResolvableArchive
A parser and serializer for PathResolvableArchive(PRA) format

## Specification

The format of PRA file is described in Table 1

<center>

Offset | Size      | Name                | Description
-------|-----------|---------------------|-----------------------------------------------
 0     | 3         | MagicNumber         | Identifier of file format
 3     | 4         | MetaDataLength(MDL) | Length of the MetaData.<br />32bit unsigned integer, little-endian.
 7     | MDL       | MetaData            | The MetaData is JSON format string.<br />The MetaData have file infos that includes filepath info etc. .
 7+MDL | until EOF | FileDataBlocks      | Buffer of the FileData arranged continuously.

**Table 1 Structure of PRA file**
</center>

The format of the MetaData as follows:

```json
{
	"version": 1,
	"files": [
		{ "path": "maps/map1.json", "offset": "0", "length": "80" },
		{ "path": "chars/char1.png", "offset": "80", "length": "160" },
		{ "path": "chars/char2.png", "offset": "240", "length": "140" }
	]
}
```

## License
MIT
