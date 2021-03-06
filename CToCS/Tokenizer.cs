﻿/*
 * FILE         : Tokenizer.cs
 * AUTHOR       : A. Saad Imran
 * FIRST VER.   : February 26, 2017
 * DESCRIPTION  :
 *  The Tokenizer class takes in a string and creates tokens from the
 * inputted string. 
 */

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CToCS
{
    public class Tokenizer
    {
        // The list TokenData stores the Regex objects which specify
        // how to identify all the different kinds of tokens
        private List<TokenData> tokenDatas = new List<TokenData>();
        // This is our list of tokens generated by the tokenizer
        private List<Token> tokens = new List<Token>();
        // This is the input text we'll tokenize
        private string inputText = null;
        // This variable stores the tokens of the input text. This array
        // is used while identifying different tokens for the list above
        private string[] lines;
        public Tokenizer(string inputText)
        {
            this.inputText = inputText;
            // Create different Regex objects for the different token types
            // If the token string contains the signature for the main method, we know that
            // the main method is being defined
            tokenDatas.Add(new TokenData(new Regex("int [\\s]*main|void [\\s]*main"), TokenType.MAIN_METHOD));
            // If the token string contains the signature for an assorted variable declaration,
            // we know a variable is being declared
            tokenDatas.Add(new TokenData(new Regex("(int|float|double) [\\s]*[\t]*((?![main])([a-zA-Z][a-zA-Z0-9]*))[\\s]*(\\;|\\=)"), TokenType.ASSORTED_VAR));
            // If the token string contains the signature for a method declaration,
            // we know a variable is being declared
            tokenDatas.Add(new TokenData(new Regex("(int|void|char[\\s]*\\*[\\s]*|float|double) [\\s]*((?![main])([a-zA-Z][a-zA-Z0-9]*))[\\s]*"), TokenType.METHOD));
            // If the token string contains the signature for a string declaration, we know that 
            // a string is being declared
            tokenDatas.Add(new TokenData(new Regex("(char[\\s]*[\t]*\\*[\\s]*|char[\\s]*[\t]*([a-zA-Z][a-zA-Z0-9])*[\\s]*(\\[[0-9]+\\])[\\s]*(;|=))"), TokenType.STRING_VAR));
            // If the token string contains the signature for a FILE variable declarion, we 
            // know a file is being declared
            tokenDatas.Add(new TokenData(new Regex("FILE[\\s]*\\*[\\s]*"), TokenType.FILE_VAR));
            // If the token string contains the signature for if or else blocks, then we know 
            // that the token type is IF_ELSE
            tokenDatas.Add(new TokenData(new Regex("if[\\s]*\\(|else[\\s]*\\("), TokenType.IF_ELSE));
            // If the token string contains the while keyword, we'll assume that we're starting a while
            // loop
            tokenDatas.Add(new TokenData(new Regex("^while"), TokenType.WHILE));
            // If the token string contains any of these function names from stdio or stdlib,
            // then we know that the token type relates to the function calls
            tokenDatas.Add(new TokenData(new Regex("atoi"), TokenType.ATOI));
            tokenDatas.Add(new TokenData(new Regex("fopen"), TokenType.FOPEN));
            tokenDatas.Add(new TokenData(new Regex("fprintf"), TokenType.FPRINTF));
            tokenDatas.Add(new TokenData(new Regex("^printf"), TokenType.PRINTF));
            tokenDatas.Add(new TokenData(new Regex("fclose"), TokenType.FCLOSE));
            tokenDatas.Add(new TokenData(new Regex("^gets"), TokenType.GETS));
            tokenDatas.Add(new TokenData(new Regex("^for"), TokenType.FOR));
            tokenDatas.Add(new TokenData(new Regex("fgets"), TokenType.FGETS));
            // If the token string contains a "+" or "=", we'll assume that the token is
            // assigning or alter variables declared within the scope of the program
            tokenDatas.Add(new TokenData(new Regex("\\+|\\="), TokenType.TOKEN));
            // If a token string contains an "{" then we're starting a block, 
            // if it contains a "}", then we're ending the block
            tokenDatas.Add(new TokenData(new Regex("{"), TokenType.START_BLOCK));
            tokenDatas.Add(new TokenData(new Regex("}"), TokenType.END_BLOCK));
            // If the token string contains the return keyword, we'll assume that the statement
            // is returning a value from the function
            tokenDatas.Add(new TokenData(new Regex("^return"), TokenType.RETURN));
            // Tokenize the inputText
            Parse();
        }

        /*
         * METHOD       : Parse()
         * DESCRIPTION  : Tokenizes the input text
         */
        public void Parse()
        {
            lines = null;
            // Split by new line
            lines = inputText.Split('\n');
            // For each token, we'll iterate through all the "TokenDatas"
            // and if the token matches Regex expression, we'll assign
            // that token type to the token. All the tokens are added
            // to the tokens list
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                TokenType type = TokenType.NONE;
                foreach (TokenData data in tokenDatas)
                {
                    if (data.GetPattern().IsMatch(lines[i]))
                    {
                        type = data.GetTokenType();
                        break;
                    }
                }
                tokens.Add(new Token(lines[i], type, this));
            }
        }

        /*
         * METHOD       : GetLines()
         * DESCRIPTION  : Gets the token list
         * RETURNS      :
         *  List<Token> - Tokenized lines from input text
         */
        public List<Token> GetLines()
        {
            return tokens;
        }
    }
}
