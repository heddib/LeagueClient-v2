var $ = $$;

$.http('/kappa/info/versions').post('[]', http => {
    $('#game').text = http.json.value.game;
    $('#patch').text = http.json.value.patch;
});