Imports Newtonsoft.Json
Imports System.Net.Http

Public Module Serum
    Public url As String = "https://serum-api.bonfida.com/"
    Public Async Function SerumResult(URI As Uri) As Task(Of String)
        Using client As HttpClient = New HttpClient
            Using response As HttpResponseMessage = Await client.GetAsync(URI)
                Using content As HttpContent = response.Content
                    Dim result As String = Await content.ReadAsStringAsync()
                    If result IsNot Nothing Then
                        Return result.ToString()
                        Exit Function
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function
    Public Async Function GetAllPairs() As Task(Of PAIRS)
        ' Provides a list of all trading pairs on the Serum DEX.
        Dim json As New PAIRS
        Dim result As String = Await SerumResult(New Uri(url & "pairs"))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of PAIRS)(result)
        End If
        Return json
    End Function
    Public Async Function GetRecentTrades(marketName As String) As Task(Of TRADES)
        ' Provides a list of all market fills from the last 24 hours on the Serum DEX.
        Dim json As New TRADES
        Dim result As String = Await SerumResult(New Uri(url & "trades/" & marketName))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of TRADES)(result)
        End If
        Return json
    End Function
    Public Async Function GetRecentTradesByMarketAddress(marketAddress As String) As Task(Of TRADES)
        ' Provides a list of all market fills from the last 24 hours on the Serum DEX.
        Dim json As New TRADES
        Dim result As String = Await SerumResult(New Uri(url & "trades/address/" & marketAddress))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of TRADES)(result)
        End If
        Return json
    End Function
    Public Async Function GetAllRecentTrades() As Task(Of TRADES)
        ' Provides a list of all market fills from the last 24 hours on the Serum DEX.
        Dim json As New TRADES
        Dim result As String = Await SerumResult(New Uri(url & "trades/all/recent"))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of TRADES)(result)
        End If
        Return json
    End Function
    Public Async Function GetVolume(marketName As String) As Task(Of VOLUMES)
        ' Provides a view of rolling 24 hour volume on the Serum DEX - use ‘all’ as the market for an aggregate of traded volume across all markets.
        Dim json As New VOLUMES
        Dim result As String = Await SerumResult(New Uri(url & "volumes/" & marketName))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of VOLUMES)(result)
        End If
        Return json
    End Function
    Public Async Function GetOrderbook(marketName As String) As Task(Of ORDERBOOKS)
        ' Provides the current orderbook of the market.
        Dim json As New ORDERBOOKS
        Dim result As String = Await SerumResult(New Uri(url & "orderbooks/" & marketName))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of ORDERBOOKS)(result)
        End If
        Return json
    End Function
    Public Async Function GetHistoricalPrices(marketName As String, Optional resolution As Integer = 86400, Optional startTime As Int64 = Nothing, Optional endTime As Int64 = Nothing, Optional limit As Integer = 1000) As Task(Of CANDLES)
        'not found
        Dim Parameters As String
        If resolution = 60 Or resolution = 3600 Or resolution = 14400 Or resolution = 86400 Then
            Parameters = "?resolution=" & resolution
        Else
            Parameters = "?resolution=86400"
        End If
        If Not startTime = Nothing Then
            Parameters = Parameters & "&startTime=" & startTime
        End If
        If Not endTime = Nothing Then
            Parameters = Parameters & "&endTime=" & endTime
        End If
        If limit < 1000 And limit > 0 Then
            Parameters = Parameters & "&limit=" & limit
        Else
            Parameters = Parameters & "&limit=1000"
        End If
        Dim json As New CANDLES
        Dim result As String = Await SerumResult(New Uri(url & "candles/" & marketName & Parameters))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of CANDLES)(result)
        End If
        Return json
    End Function
    Public Async Function GetAllPool() As Task(Of POOLS)
        'Provides data about Serum pools over the last 6 hours. A new data point is added every 30 minutes for each pool.
        Dim json As New POOLS
        Dim result As String = Await SerumResult(New Uri(url & "pools"))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLS)(result)
        End If
        Return json
    End Function
    Public Async Function GetAllPoolRecent() As Task(Of POOLS)
        Dim json As New POOLS
        Dim result As String = Await SerumResult(New Uri(url & "pools-recent"))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLS)(result)
        End If
        Return json
    End Function
    Public Async Function GetPool(mintA As String, mintB As String, Optional startTime As Int64 = Nothing, Optional endTime As Int64 = Nothing, Optional limit As Integer = 1000) As Task(Of POOLS)
        'Provides historical data about Serum pools
        Dim Parameters As String = mintA & "/" & mintB
        Dim s As String = "?"
        If Not startTime = Nothing Then
            Parameters = Parameters & s & "startTime=" & startTime
            s = "&"
        End If
        If Not endTime = Nothing Then
            Parameters = Parameters & s & "endTime=" & endTime
            s = "&"
        End If
        If limit < 1000 And limit > 0 Then
            Parameters = Parameters & s & "limit=" & limit
        End If
        Dim json As New POOLS
        Dim result As String = Await SerumResult(New Uri(url & "pools/" & Parameters))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLS)(result)
        End If
        Return json
    End Function
    Public Async Function GetPoolTades(Optional symbolSource As String = Nothing, Optional symbolDestination As String = Nothing, Optional bothDirections As Boolean = Nothing) As Task(Of POOLSTRADES)
        ' Provides a list of all trades fills from the last 24 hours on the Serum Swap.
        Dim Parameters As String = ""
        Dim s As String = "?"
        If Not symbolSource = Nothing Then
            Parameters = "?symbolSource=" & symbolSource
            s = "&"
        End If
        If Not symbolDestination = Nothing Then
            Parameters = Parameters & s & "symbolDestination=" & symbolDestination
            s = "&"
        End If
        If Not bothDirections = Nothing Then
            Parameters = Parameters & s & "bothDirections=" & bothDirections.ToString.ToLower
        End If
        Dim json As New POOLSTRADES
        Dim result As String = Await SerumResult(New Uri(url & "/pools/trades" & Parameters))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLSTRADES)(result)
        End If
        Return json
    End Function
    Public Async Function GetPoolsLast24hVolume() As Task(Of POOLS24HVOLUME)
        Dim json As New POOLS24HVOLUME
        Dim result As String = Await SerumResult(New Uri(url & "/pools/volumes/recent"))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLS24HVOLUME)(result)
        End If
        Return json
    End Function
    Public Async Function GetPoolsHistoricalVolume(mintA As String, mintB As String, Optional startTime As Int64 = Nothing, Optional endTime As Int64 = Nothing, Optional limit As Integer = 100) As Task(Of POOLSVOLUMES)
        'Provides historical volume data for pools.
        Dim Parameters As String = mintA & "&mintB=" & mintB
        If Not startTime = Nothing Then
            Parameters = Parameters & "&startTime=" & startTime
        End If
        If Not endTime = Nothing Then
            Parameters = Parameters & "&endTime=" & endTime
        End If
        If limit < 100 And limit > 0 Then
            Parameters = Parameters & "&limit=" & limit
        End If
        Dim json As New POOLSVOLUMES
        Dim result As String = Await SerumResult(New Uri(url & "pools/volumes?mintA=" & Parameters))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLSVOLUMES)(result)
        End If
        Return json
    End Function
    Public Async Function GetPoolsHistoricalLiquidity(mintA As String, mintB As String, Optional startTime As Int64 = Nothing, Optional endTime As Int64 = Nothing, Optional limit As Integer = 100) As Task(Of POOLSLIQUIDITY)
        'Provides historical liquidity data for pools.
        Dim Parameters As String = mintA & "&mintB=" & mintB
        If Not startTime = Nothing Then
            Parameters = Parameters & "&startTime=" & startTime
        End If
        If Not endTime = Nothing Then
            Parameters = Parameters & "&endTime=" & endTime
        End If
        If limit < 100 And limit > 0 Then
            Parameters = Parameters & "&limit=" & limit
        End If
        Dim json As New POOLSLIQUIDITY
        Dim result As String = Await SerumResult(New Uri(url & "pools/liquidity?mintA=" & Parameters))
        If result IsNot Nothing Then
            json = JsonConvert.DeserializeObject(Of POOLSLIQUIDITY)(result)
        End If
        Return json
    End Function
End Module
Public Class PAIRS
    Public success As Boolean = False
    Public data() As String
End Class
Public Class TRADES
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public market As String
        Public price As Double
        Public size As Double
        Public side As String
        Public time As Int64
        Public orderId As String
        Public feeCost As Double
        Public marketAddress As String
    End Class
End Class
Public Class VOLUMES
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public volumeUsd As Double
        Public volume As Double
    End Class
End Class
Public Class ORDERBOOKS
    Public success As Boolean = False
    Public data As _DATA
    Class _DATA
        Public market As String
        Public bids As List(Of _book)
        Public asks As List(Of _book)
        Public marketAddress As String
        Class _book
            Public price As Double
            Public size As Double
        End Class
    End Class
End Class

Public Class CANDLES
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public close As Double
        Public open As Double
        Public low As Double
        Public high As Double
        Public startTime As Int64
        Public market As String
        Public volumeBase As Double
        Public volumeQuote As Double
    End Class
End Class

Public Class POOLS
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public name As String
        Public pool_identifier As String
        Public liquidity_locked As Double
        Public apy As Double
        Public volume As Double
        Public mints() As String
        Public liquidityA As Double
        Public liquidityAinUsd As Double
        Public liquidityB As Double
        Public liquidityBinUsd As Double
        Public supply As Double
        Public fees As Double
        Public time As Int64
        Public volume24hA As Double
        Public volume24hB As Double
    End Class
End Class

Public Class POOLSTRADES
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public signature As String
        Public symbolSource As String
        Public poolSourceMint As String
        Public symbolDestination As String
        Public poolDestinationMint As String
        Public amountIn As Double
        Public amountOut As Double
        Public poolMintAuthority As String
        Public time As Int64
    End Class
End Class

Public Class POOLS24HVOLUME
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public volume As Double
    End Class
End Class

Public Class POOLSVOLUMES
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public mintA As String
        Public mintB As String
        Public volume As Double
        Public time As Int64
    End Class
End Class

Public Class POOLSLIQUIDITY
    Public success As Boolean = False
    Public data As List(Of _DATA)
    Class _DATA
        Public mintA As String
        Public mintB As String
        Public liquidityAinUsd As Double
        Public liquidityBinUsd As Double
        Public liquidityA As Double
        Public liquidityB As Double
        Public time As Int64
    End Class
End Class
