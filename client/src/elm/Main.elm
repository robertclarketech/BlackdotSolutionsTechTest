module Main exposing (main)

import Browser
import Html exposing (Html, a, article, button, div, h1, h2, input, nav, p, progress, table, tbody, td, text, th, thead, tr)
import Html.Attributes exposing (class, placeholder, type_, value)
import Html.Events exposing (onClick, onInput)
import Http
import Json.Decode as Decode exposing (Decoder)
import Json.Decode.Pipeline exposing (optional, required)
import Url.Builder exposing (crossOrigin)


main : Program Flags Model Msg
main =
    Browser.element
        { init = init
        , update = update
        , subscriptions = subscriptions
        , view = view
        }


init : () -> ( Model, Cmd Msg )
init _ =
    ( Model [] Nothing Nothing "" False, getAllQueries )



-- TYPES


type alias Model =
    { allQueries : List Query
    , selectedQuery : Maybe Query
    , error : Maybe String
    , newQuery : String
    , performingQuery : Bool
    }


type alias Query =
    { query : String
    , queryResults : List QueryResult
    }


type alias QueryResult =
    { text : String
    , url : String
    , searchEngineType : String
    }


type Msg
    = NoOp
    | GotAllQueries (Result Http.Error (List Query))
    | ClickQuery Query
    | GotQuery (Result Http.Error Query)
    | RemoveErrorMessage
    | QueryTextInput String
    | PerformQuery String
    | FinishedQuery (Result Http.Error ())


type alias Flags =
    ()



-- SUBSCRIPTIONS


subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none



-- UPDATE


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        NoOp ->
            ( model, Cmd.none )

        GotAllQueries result ->
            case result of
                Ok queries ->
                    ( { model | allQueries = queries }, Cmd.none )

                Err error ->
                    let
                        errorText =
                            formatHttpError error
                    in
                    ( { model | error = Just errorText }, Cmd.none )

        ClickQuery query ->
            ( { model | selectedQuery = Just query }, getQuery query.query )

        GotQuery result ->
            case result of
                Ok query ->
                    ( { model | selectedQuery = Just query }, Cmd.none )

                Err error ->
                    let
                        errorText =
                            formatHttpError error
                    in
                    ( { model | error = Just errorText }, Cmd.none )

        RemoveErrorMessage ->
            ( { model | error = Nothing }, Cmd.none )

        QueryTextInput text ->
            ( { model | newQuery = text }, Cmd.none )

        PerformQuery queryText ->
            ( { model | performingQuery = True }, performQuery queryText )

        FinishedQuery result ->
            case result of
                Ok _ ->
                    ( { model | newQuery = "", performingQuery = False }, Cmd.batch [ getQuery model.newQuery, getAllQueries ] )

                Err error ->
                    let
                        errorText =
                            formatHttpError error
                    in
                    ( { model | error = Just errorText, performingQuery = False }, Cmd.none )


formatHttpError : Http.Error -> String
formatHttpError error =
    case error of
        Http.BadUrl badUrlString ->
            badUrlString

        Http.Timeout ->
            "Http Request Timed Out"

        Http.NetworkError ->
            "Network Error - please check your connection"

        Http.BadStatus int ->
            "Request failed with a status of " ++ String.fromInt int

        Http.BadBody badBodyString ->
            badBodyString



-- HTTP


baseUrl : String
baseUrl =
    "https://localhost:5001"


getAllQueriesUrl : String
getAllQueriesUrl =
    crossOrigin baseUrl [ "SearchEngineInvestigate" ] []


getQueryUrl : String -> String
getQueryUrl query =
    crossOrigin getAllQueriesUrl [ query ] []


postQueryUrl : String -> String
postQueryUrl query =
    crossOrigin getAllQueriesUrl [ query ] []



-- ALL QUERIES


getAllQueries : Cmd Msg
getAllQueries =
    Http.get
        { url = getAllQueriesUrl
        , expect = Http.expectJson GotAllQueries (Decode.list queryDecoder)
        }


queryDecoder : Decoder Query
queryDecoder =
    Decode.succeed Query
        |> required "query" Decode.string
        |> optional "queryResults" (Decode.list queryResultDecoder) []


queryResultDecoder : Decoder QueryResult
queryResultDecoder =
    Decode.succeed QueryResult
        |> required "resultText" Decode.string
        |> required "resultLink" Decode.string
        |> required "searchEngineType" Decode.string



-- ONE QUERY


getQuery : String -> Cmd Msg
getQuery query =
    Http.get
        { url = getQueryUrl query
        , expect = Http.expectJson GotQuery queryDecoder
        }



-- PERFORM QUERY


performQuery : String -> Cmd Msg
performQuery query =
    Http.post
        { url = postQueryUrl query
        , body = Http.emptyBody
        , expect = Http.expectWhatever FinishedQuery
        }



--VIEW


view : Model -> Html Msg
view model =
    div []
        [ case model.performingQuery of
            True ->
                div [ class "bigLoading" ]
                    [ div [ class "container auto-margin" ]
                        [ h1 [ class "title" ] [ text "Please wait for the query to finish running" ]
                        , progress [ class "progress is-large is-info" ] []
                        ]
                    ]

            False ->
                text ""
        , div [ class "container" ]
            [ case model.error of
                Just errorMessage ->
                    article [ class "message is-danger" ]
                        [ div [ class "message-header" ]
                            [ p [] [ text "Error" ]
                            , button [ class "delete", onClick RemoveErrorMessage ] []
                            ]
                        , div [ class "message-body" ] [ text errorMessage ]
                        ]

                Nothing ->
                    text ""
            , div [ class "section" ]
                [ h2 [ class "title" ] [ text "Perform Query" ]
                , div [ class "field has-addons" ]
                    [ div [ class "control" ]
                        [ input [ class "input", type_ "text", placeholder "Type your query here", onInput QueryTextInput, value model.newQuery ] [] ]
                    , button [ class "button is-info", onClick (PerformQuery model.newQuery) ] [ text "Search" ]
                    , div [ class "loading" ] []
                    ]
                ]
            , div [ class "section" ]
                [ nav [ class "panel" ]
                    ([ p [ class "panel-heading" ] [ text "All Queries" ] ] ++ List.map viewQueryButton model.allQueries)
                ]
            , case model.selectedQuery of
                Just query ->
                    div [ class "section" ]
                        [ h2 [ class "title" ] [ text <| "Viewing Query: " ++ query.query ]
                        , table [ class "table" ]
                            [ thead []
                                [ tr []
                                    [ th [] [ text "Link Text" ]
                                    , th [] [ text "Link" ]
                                    , th [] [ text "Search Engine" ]
                                    ]
                                ]
                            , tbody [] (List.map viewQueryResult query.queryResults)
                            ]
                        ]

                Nothing ->
                    text ""
            ]
        ]


viewQueryButton : Query -> Html Msg
viewQueryButton query =
    a [ class "panel-block", onClick (ClickQuery query) ] [ text query.query ]


viewQueryResult : QueryResult -> Html Msg
viewQueryResult queryResult =
    tr []
        [ td [] [ text queryResult.text ]
        , td [] [ a [] [ text queryResult.url ] ]
        , td [] [ text queryResult.searchEngineType ]
        ]
