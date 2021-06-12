namespace Expresser.Parsing
{
	public class Parser
	{
		public IParserLanguage Language { get; }

		public Parser(IParserLanguage language)
		{
			Language = language;
		}

		public ParserNodeChildrenCollectionEumerator EnumerateChildren(ParserNode node)
		{
			return new ParserNodeChildrenCollectionEumerator(node.TokenId, node.NodeType);
		}
	}

	public struct ParserNode
	{
		public int TokenId { get; set; }
		public int NodeType { get; set; }
	}

	public struct ParserNodeChildrenCollectionEumerator
	{
		internal int tokenId;
		internal int nodeType;

		internal ParserNodeChildrenCollectionEumerator(int tokenId, int nodeType)
		{
			this.tokenId = tokenId;
			this.nodeType = nodeType;
		}

		public ParserNode Current
		{
			get
			{
				throw new System.NotImplementedException();
			}
		}

		public bool MoveNext()
		{
			throw new System.NotImplementedException();
		}

		public void Reset()
		{
			throw new System.NotImplementedException();
		}
	}

}
