var $ = $$;

$.http('/kappa/info/versions').post('[]', http => {
    $('#patch').text = 'Patch ' + http.json.value.patch;
    $('#game').text = 'Game Client: ' + http.json.value.game;
    $('#wad').text = 'Game Assets: ' + http.json.value.wad;
});